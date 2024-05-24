using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CleanAspCore.Telemetry;

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
