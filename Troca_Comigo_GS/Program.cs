using HealthChecks.UI.Client;
using Infrastructure.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics;
using System.Text;

// ==== LOGGING & TRACING ====
using Serilog;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

// =========================
// 🔵 SERILOG (LOGGING ESTRUTURADO)
// =========================

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console() // Log no console
    .WriteTo.File("logs/api-log-.txt", rollingInterval: RollingInterval.Day) // Log diário em arquivo
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Integrar Serilog ao Host
builder.Host.UseSerilog();

// =========================
// 🔵 TRACING (OpenTelemetry)
// =========================

// ActivitySource -> Necessário para rastrear operações internas
var activitySource = new ActivitySource("TrocaComigoAPI");

builder.Services.AddSingleton(activitySource);

builder.Services.AddOpenTelemetry()
    .WithTracing(t =>
    {
        t
        .AddSource("TrocaComigoAPI")
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault().AddService("TrocaComigoAPI")
        )
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation();
    });

// =========================
// 🔵 API VERSIONING
// =========================
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// =========================
// 🔵 JWT AUTH
// =========================
var jwtSection = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidAudience = jwtSection["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Key"]!)
            ),
            ClockSkew = TimeSpan.Zero
        };
    });

// =========================
// 🔵 IOC
// =========================
builder.Services.AddIoC(builder.Configuration);

// =========================
// 🔵 CONTROLLERS
// =========================
builder.Services.AddControllers();

// =========================
// 🔵 SWAGGER
// =========================
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Troca Comigo API",
        Version = "v1",
        Description = "API do Global Solution FIAP – Skill Swap Hub"
    });

    options.EnableAnnotations();
    options.ExampleFilters();

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira apenas o token JWT. O prefixo 'Bearer' será adicionado automaticamente.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

// =========================
// 🔵 APP
// =========================
var app = builder.Build();

// Middleware de Logging (Serilog)
app.UseSerilogRequestLogging();

// Segurança
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// =========================
// 🔵 SWAGGER
// =========================
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Troca Comigo API v1");
});

// =========================
// 🔵 HEALTH CHECK ENDPOINTS
// =========================
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = h => h.Tags.Contains("live")
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = h => h.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Controllers
app.MapControllers();

app.Run();

// 🔥 NECESSÁRIO PARA TESTES DE INTEGRAÇÃO FUNCIONAREM
public partial class Program { }
