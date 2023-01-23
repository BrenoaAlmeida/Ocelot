using JwtAuthenticationManager;
using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.ApiGateway;
using Ocelot.Configuration.File;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: false)
    .AddEnvironmentVariables();

//builder.Services.AddAuthentication(options => {
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//    .AddJwtBearer(options => { 
//        options.RequireHttpsMetadata  = false;
//        options.SaveToken = true;
//        options.Configuration.
//    });

//builder.Services.AddAuthentication().AddJwtBearer("Bearer", options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters()
//    {
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("f:'FC9Yx9_M%`&4u[7p!:^'V]kU9p5@%\"({\"2j7S")),
//        ValidAudience = "Audience",
//        ValidIssuer = "Issuer",
//        ValidateIssuerSigningKey = true,
//        ValidateLifetime = true,
//        ClockSkew = TimeSpan.Zero
//    };   
//});

builder.Services.AddOcelot().AddDelegatingHandler<HeaderDelegatingHandler>();

builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.UseOcelot();

app.Run();
