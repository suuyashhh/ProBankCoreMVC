using ProBankCoreMVC.Contest;
using ProBankCoreMVC.Interfaces;
using ProBankCoreMVC.Repositries;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<ILogin, LoginRepository>();






// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(
ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false")
);

// Dapper + EF Core
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddDbContext<ProBankCoreMVCDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("myTestDB")));

var app = builder.Build();

// Configure middleware pipeline
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// No Authentication / JWT
app.UseAuthorization();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapControllers();

app.Run();
