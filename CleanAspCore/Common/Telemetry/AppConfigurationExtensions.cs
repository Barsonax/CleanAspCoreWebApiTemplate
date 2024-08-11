using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CleanAspCore.Common.Telemetry;

public static class AppConfigurationExtensions
{
    internal static void AddOpenTelemetryServices(this WebApplicationBuilder builder)
    {
        if (builder.Configuration.GetValue<bool?>("DisableTelemetry") == true)
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
