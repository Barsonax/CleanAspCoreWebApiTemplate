using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CleanAspCore.Core.Common.Telemetry;

public static class AppConfigurationExtensions
{
    public static void AddOpenTelemetryServices(this WebApplicationBuilder builder)
    {
        if (!builder.Configuration.GetValue<bool>("EnableTelemetry"))
            return;

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.IncludeScopes = true;
            options.AddProcessor(new EnrichLogsProcessor());
        });
        var telemetryBuilder = builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(
                builder.Environment.ApplicationName,
                serviceInstanceId: builder.Environment.IsDevelopment() ? builder.Environment.ApplicationName : null))
            .WithTracing(tracing => tracing
                .AddSource(Instrumentation.ActivitySource.Name)
                .AddProcessor(new EnrichSpanProcessor())
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddEntityFrameworkCoreInstrumentation())
            .WithMetrics(metrics => metrics
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddMeter(Instrumentation.Meter.Name));

        if (builder.Environment.IsDevelopment())
        {
            telemetryBuilder.UseOtlpExporter();
        }
        else
        {
            telemetryBuilder.UseAzureMonitor();
        }
    }
}
