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

builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<ILogin, LoginRepository>();

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false")
);

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddDbContext<ProBankCoreMVCDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("myTestDB")));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt"); // ? FIXED

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false, // ? Token never expires unless manually blacklisted
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            )
        };
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthentication();                        // ? Auth first
app.UseMiddleware<ProBankCoreMVC.Controllers.LoginController.TokenValidationMiddleware>(); // ? Then your Redis session middleware
app.UseAuthorization();


app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapControllers();

app.Run();
