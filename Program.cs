using Microsoft.EntityFrameworkCore;
using WinLimitAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MongoDB.Driver;
using AspNetCore.Identity.MongoDbCore.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddOpenApi();

builder.Services.AddControllers();

// MongoDB settings from app settings
string mongoConnectionString = builder.Configuration.GetConnectionString("MongoConnection");
Console.WriteLine(mongoConnectionString);
string mongoDatabaseName = builder.Configuration["MongoSettings:DatabaseName"];

builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));

builder.Services.AddIdentity<User, MongoIdentityRole<Guid>>()
    .AddMongoDbStores<User, MongoIdentityRole<Guid>, Guid>(
        mongoConnectionString,
        mongoDatabaseName
    )
    .AddDefaultTokenProviders();

IConfigurationSection jwtSettings = builder.Configuration.GetSection("JwtSettings");
byte[] key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

