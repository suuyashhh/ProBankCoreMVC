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

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var userTokenStore = new ConcurrentDictionary<string, string>();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling =
            Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI (your original)
builder.Services.AddScoped<ILogin, LoginRepository>();
builder.Services.AddScoped<IBranchMast, BranchMastRepository>();
builder.Services.AddScoped<ICountryMaster, CountryMasterRepository>();
builder.Services.AddScoped<IStateMaster, StateMasterRepository>();
builder.Services.AddScoped<IDistrictMaster, DistrictMasterRepository>();
builder.Services.AddScoped<ITalukaMaster, TalukaMasterRepository>();
builder.Services.AddScoped<ICityMaster, CityMasterRepository>();
builder.Services.AddScoped<IAreaMaster, AreaMasterRepository>();
builder.Services.AddScoped<ICastMaster, CastMasterRepository>();
builder.Services.AddScoped<IReligionMaster, ReligionMasterRepository>();
builder.Services.AddScoped<IOccupationMaster, OccupationMasterRepository>();
builder.Services.AddScoped<IValidationService, ValidationServiceRepository>();
builder.Services.AddScoped<IUserMenuAccess, UserMenuAccessRepository>();
builder.Services.AddScoped<IPartyMaster, PartyMasterRepository>();
builder.Services.AddScoped<IDireMast, DireMastRepository>();
builder.Services.AddScoped<IStaffMaster, StaffMasterRepository>();
builder.Services.AddScoped<IKycAddressMaster, KycAddressMasterRepository>();
builder.Services.AddScoped<IKycIdMaster, KycIdMasterRepository>();
builder.Services.AddScoped<IAccountTypeMaster, AccountTypeMasterRepository>();
builder.Services.AddScoped<IPrefixMaster, PrefixMasterRespository>();
builder.Services.AddScoped<IAgentMaster, AgentMasterRepository>();
builder.Services.AddScoped<IThreeFieldMaster, ThreeFieldMasterRepository>();    
builder.Services.AddScoped<ICommanMaster, CommanMasterRepository>();    
builder.Services.AddScoped<IDesignationMaster, DesignationRepository>();
builder.Services.AddScoped<IDepositeAccountOpening, DepositeAccountOpeningRepository>();
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false")
);

builder.Services.AddSingleton<PhotoDapperContext>();
builder.Services.AddDbContext<ProBankCoreMVCDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connPhotoSign"))
);

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddDbContext<ProBankCoreMVCDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connString"))
);

builder.Services.AddSingleton(userTokenStore);

var angularDev = builder.Configuration["Cors:AngularOriginDev"];
var angularProd = builder.Configuration["Cors:AngularOriginProd"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularOnly", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200", "https://localhost:4200",
                "http://103.93.97.222:1552",
                "https://103.93.97.222:1552"
            )
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");
        var key = jwt["Key"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHsts();

app.UseCors("AngularOnly");

app.UseAuthentication();

// 🧱 LAYER 1 — IP ALLOW LIST
//app.UseMiddleware<IpAllowListMiddleware>();

// 🧱 LAYER 2 — APP KEY VALIDATION
//app.UseMiddleware<AppKeyValidationMiddleware>();

// 🧱 LAYER 3 — HMAC SIGNATURE VALIDATION
//app.UseMiddleware<HmacValidationMiddleware>();

// 🧱 LAYER 4 — SINGLE SESSION PROTECTOR
//app.UseMiddleware<TokenValidationMiddleware>(userTokenStore);

app.UseAuthorization();

app.MapControllers();

app.Run();
