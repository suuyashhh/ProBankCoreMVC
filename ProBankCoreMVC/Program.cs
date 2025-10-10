using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;
using ProBankCoreMVC.Repositries;
using System.Security.Claims;
using System.Text;
using StackExchange.Redis;
using ProBankCoreMVC.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration: DapperContext and Repositories
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<ILogin, LoginRepository>();


// Redis (optional - keep if you run Redis locally). If not using, comment this out.
// builder.Services.AddSingleton<IConnectionMultiplexer>(
//     ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false")
// );

// EF Core DbContext (optional, you may not use EF at the moment)
builder.Services.AddDbContext<ProBankCoreMVCDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connString")));

// JWT configuration from appsettings.json "Jwt" section
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection.GetValue<string>("Key");
if (string.IsNullOrEmpty(jwtKey))
{
    throw new Exception("JWT key is not configured in appsettings.json (Jwt:Key).");
}

// Configure Authentication - JWT Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection.GetValue<string>("Issuer"),
        ValidAudience = jwtSection.GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };

    // Single-device enforcement: validate token's JTI against stored value in DB
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var principal = context.Principal;
            var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var jti = principal?.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti)?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(jti))
            {
                context.Fail("Invalid token claims.");
                return;
            }

            // Resolve ILogin repository to get stored JTI for this user
            var loginRepo = context.HttpContext.RequestServices.GetRequiredService<ILogin>();
            var storedJti = await loginRepo.GetUserJtiAsync(userId);
            if (string.IsNullOrEmpty(storedJti) || storedJti != jti)
            {
                context.Fail("Token revoked or replaced by another session.");
            }
        }
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add authentication BEFORE authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapControllers();

app.Run();
