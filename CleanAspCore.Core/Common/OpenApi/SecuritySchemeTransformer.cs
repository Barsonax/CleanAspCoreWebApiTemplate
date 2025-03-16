﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;

namespace CleanAspCore.Core.Common.OpenApi;

internal sealed class SecuritySchemeTransformer(IConfiguration configuration, IWebHostEnvironment environment) : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var config = configuration.GetRequiredSection(Constants.AzureAd).Get<MicrosoftIdentityOptions>()!;

        var requirements = new Dictionary<string, OpenApiSecurityScheme>
        {
            ["AzureAd"] = new()
            {
                Type = SecuritySchemeType.OAuth2,
                Scheme = "bearer", // "bearer" refers to the header name here
                In = ParameterLocation.Header,
                BearerFormat = "Json Web Token",
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"https://login.microsoftonline.com/{config.TenantId}/oauth2/v2.0/authorize"),
                        TokenUrl = new Uri($"https://login.microsoftonline.com/{config.TenantId}/oauth2/v2.0/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { $"api://{config.ClientId}/default", "read" },
                        },
                        Extensions = new Dictionary<string, IOpenApiExtension> { { "x-usePkce", new OpenApiString("SHA-256") } },
                    }
                }
            }
        };

        if (environment.IsDevelopment())
        {
            requirements.Add(JwtBearerDefaults.AuthenticationScheme, new()
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer", // "bearer" refers to the header name here
                In = ParameterLocation.Header,
                BearerFormat = "Json Web Token"
            });
        }

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = requirements;

        foreach (var operation in document.Paths.Values.SelectMany(path => path.Operations))
        {
            operation.Value.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "AzureAd",
                        Type = ReferenceType.SecurityScheme
                    },
                }] = []
            });
        }

        return Task.CompletedTask;
    }
}
