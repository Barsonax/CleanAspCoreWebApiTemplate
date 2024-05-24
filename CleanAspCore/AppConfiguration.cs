using System.Reflection;
using CleanAspCore.Endpoints.Departments;
using CleanAspCore.Endpoints.Employees;
using CleanAspCore.Endpoints.Jobs;
using CleanAspCore.Telemetry;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CleanAspCore;

internal static class AppConfiguration
{
    internal static void AddAppServices(this WebApplicationBuilder builder)
    {
        builder.AddEmployeeServices();
    }

    internal static void AddAppRoutes(this IEndpointRouteBuilder host)
    {
        host.AddDepartmentsRoutes();
        host.AddEmployeesRoutes();
        host.AddJobsRoutes();
    }

    internal static void AddAuthServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddFallbackPolicy("Fallback", new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);
    }

    internal static void AddSwaggerServices(this WebApplicationBuilder builder)
    {
        if (builder.Configuration.GetValue<bool?>("DisableOpenApi") == true) return;

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SupportNonNullableReferenceTypes();

            var xmlDocPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
            options.IncludeXmlComments(xmlDocPath);

            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                BearerFormat = "JWT",
                Name = "JWT Authentication",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
        });
        builder.Services.AddFluentValidationRulesToSwagger();
    }

    internal static void AddOpenTelemetryServices(this WebApplicationBuilder builder)
    {
        if (builder.Configuration.GetValue<bool?>("DisableTelemetry") == true) return;

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeScopes = true;
            options.AddProcessor(new EnrichLogsProcessor());
            options
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(builder.Environment.ApplicationName))
                .AddOtlpExporter();
        });
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
            .WithTracing(tracing => tracing
                .AddProcessor(new EnrichSpanProcessor())
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddOtlpExporter())
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter());
    }
}
