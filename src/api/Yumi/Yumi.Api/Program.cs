using System.Security.Cryptography.X509Certificates;
using System.Text;
using Baz.Service.Action.AspNetCore.Extensions;
using Baz.Service.Action.Core;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Raven.DependencyInjection;
using Serilog;
using Yumi.Application.Configuration;
using Yumi.Application.Services;
using Yumi.Application.Validation.Dto.Commands;
using Yumi.Infrastructure.Models;
using Yumi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});
builder.Host.UseContentRoot(Directory.GetCurrentDirectory());
builder.Host.ConfigureAppConfiguration((context, configurationBuilder) =>
{
    configurationBuilder
        .SetBasePath(context.HostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true)
        .AddEnvironmentVariables();
});

var responseMapBuilder = new ServiceActionResponseMapBuilder();
builder
    .Services
    .AddControllers(options => { options.Filters.AddServiceActionFilter(responseMapBuilder.Build()); })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    })
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateRecipeCommandValidator>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGenericRepository<Recipe>, GenericRepository<Recipe>>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

var yumiConfigSection = builder.Configuration.GetSection(nameof(YumiConfiguration));
builder.Services.AddOptions<YumiConfiguration>()
    .Bind(yumiConfigSection)
    .ValidateDataAnnotations();

var yumiConfig = yumiConfigSection.Get<YumiConfiguration>();
var tokenSecretVar = Environment.GetEnvironmentVariable("TOKEN_SECRET");
if (!string.IsNullOrWhiteSpace(tokenSecretVar))
{
    yumiConfig.TokenSecret = tokenSecretVar;
}

var appHost = Environment.GetEnvironmentVariable("APP_HOST");
if (!string.IsNullOrWhiteSpace(appHost))
{
    yumiConfig.AllowedHost = appHost;
}

var respectedUserAccounts = Environment.GetEnvironmentVariable("RESPECTED_USER_ACCOUNTS");
if (!string.IsNullOrWhiteSpace(respectedUserAccounts))
{
    var accountsCollection = respectedUserAccounts.Split(';');
    if (!accountsCollection.Any())
    {
        throw new ArgumentException("Invalid config file. Unsupported format for user accounts");
    }

    yumiConfig.RespectedUserAccounts = accountsCollection;
}

var ravenDbConnectionString = Environment.GetEnvironmentVariable("RAVENDB_CONNECTION_STRING");
var ravenDbName = Environment.GetEnvironmentVariable("RAVENDB_NAME");
var ravenDbCertificate = Environment.GetEnvironmentVariable("RAVENDB_CERTIFICATE");
var ravenDbCertPassword = Environment.GetEnvironmentVariable("RAVENDB_CERTIFICATE_PASSWORD");
if (!string.IsNullOrWhiteSpace(ravenDbConnectionString)
    && !string.IsNullOrWhiteSpace(ravenDbName)
    && !string.IsNullOrWhiteSpace(ravenDbCertificate))
{
    builder.Services.AddRavenDbDocStore(options =>
    {
        var certData = Convert.FromBase64String(ravenDbCertificate);
        options.Certificate = string.IsNullOrWhiteSpace(ravenDbCertPassword)
            ? new X509Certificate2(certData)
            : new X509Certificate2(certData, ravenDbCertPassword);
        options.Settings ??= new RavenSettings();

        options.Settings.Urls = new[] { ravenDbConnectionString };
        options.Settings.DatabaseName = ravenDbName;
    });
}
else
{
    builder.Services.AddRavenDbDocStore();
}

builder.Services.AddRavenDbAsyncSession();
builder.Services.AddSingleton(yumiConfig);

builder
    .Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        var secretBytes = Encoding.ASCII.GetBytes(yumiConfig.TokenSecret);
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        };
    });

builder.Services.AddHttpClient();

var app = builder.Build();
var hosts = yumiConfig.AllowedHost.Split(";");

app.UseCors(options =>
    options.WithOrigins(hosts)
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .AllowAnyMethod()
        .AllowAnyHeader());

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();