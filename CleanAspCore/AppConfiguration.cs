using System.Reflection;
using CleanAspCore.Data;
using CleanAspCore.Endpoints.Departments;
using CleanAspCore.Endpoints.Employees;
using CleanAspCore.Endpoints.Jobs;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

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

        var authBuilder = builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
        if (builder.Environment.IsDevelopment())
        {
            authBuilder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme);
        }
        else
        {
            authBuilder.AddMicrosoftIdentityWebApi(builder.Configuration);
        }
    }

    internal static void AddOpenApiServices(this WebApplicationBuilder builder)
    {
        if (builder.Configuration.GetValue<bool?>("DisableOpenApi") == true)
            return;

        var config = builder.Configuration.GetRequiredSection(Constants.AzureAd).Get<MicrosoftIdentityOptions>()!;
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
                Type = builder.Environment.IsDevelopment() ? SecuritySchemeType.Http : SecuritySchemeType.OAuth2,
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                },
                Flows = new OpenApiOAuthFlows()
                {
                    AuthorizationCode = new OpenApiOAuthFlow()
                    {
                        AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{config.TenantId}/oauth2/v2.0/authorize"),
                        TokenUrl = new Uri($"https://login.microsoftonline.com/{config.TenantId}/oauth2/v2.0/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { $"api://{config.ClientId}/default", "read" },
                        },
                    }
                }
            };

            options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

            options.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
        });
        builder.Services.AddFluentValidationRulesToSwagger();
    }

    internal static void UseOpenApi(this WebApplication app)
    {
        if (app.Configuration.GetValue<bool?>("DisableOpenApi") == true)
            return;
        var config = app.Configuration.GetRequiredSection(Constants.AzureAd).Get<MicrosoftIdentityOptions>()!;
        app.UseSwagger();
        app.UseSwaggerUI(setup =>
        {
            setup.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");
            setup.OAuthClientId(config.ClientId);
            setup.OAuthUsePkce();
            setup.OAuthScopes($"api://{config.ClientId}/default");
        });
    }

    internal static void RunMigrations(this WebApplication app)
    {
        if (app.Configuration.GetValue<bool?>("DisableMigrations") == true)
            return;

        if (app.Environment.IsDevelopment())
        {
            var watchIteration = app.Configuration.GetValue("DOTNET_WATCH_ITERATION", 1);
            if (watchIteration == 1)
            {
                app.EnsureHrContextDatabaseIsCreated();
            }
        }
        else
        {
            app.MigrateHrContextDatabase();
        }
    }
}
