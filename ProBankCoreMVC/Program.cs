using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProBankCoreMVC.Controllers;
using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;
using ProBankCoreMVC.Repositries;
using StackExchange.Redis;
using System.Collections.Concurrent;
using System.Text;
using static ProBankCoreMVC.Controllers.LoginController;

var builder = WebApplication.CreateBuilder(args);

// ✅ Shared dictionary to track active sessions
var userTokenStore = new ConcurrentDictionary<string, string>();

// -------------------------
// 1️⃣ Services Configuration
// -------------------------
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency injection
builder.Services.AddScoped<ILogin, LoginRepository>();
builder.Services.AddScoped<IBranchMast, BranchMastRepository>();
builder.Services.AddScoped<ICountryMaster, CountryMasterRepository>();
builder.Services.AddScoped<IStateMaster, StateMasterRepository>();
builder.Services.AddScoped<IDistrictMaster, DistrictMasterRepository>();
builder.Services.AddScoped<ITalukaMaster, TalukaMasterRepository>();
builder.Services.AddScoped<ICityMaster, CityMasterRepository>();
builder.Services.AddScoped<IAreaMaster, AreaMasterRepository>();

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false")
);

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddDbContext<ProBankCoreMVCDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connString"))
);

// Register shared token store (singleton)
builder.Services.AddSingleton(userTokenStore);

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false, // No expiry for now
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            )
        };
    });

// Enable CORS
builder.Services.AddCors(policy =>
{
    policy.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

var app = builder.Build();

// -------------------------
// 2️⃣ Middleware Pipeline
// -------------------------
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseMiddleware<TokenValidationMiddleware>(userTokenStore); // ✅ Single-session middleware
app.UseAuthorization();

app.MapControllers();

app.Run();
