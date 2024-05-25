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
            options.AddOtlpExporter();
        });
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(
                builder.Environment.ApplicationName,
                serviceInstanceId: builder.Environment.IsDevelopment() ? builder.Environment.ApplicationName : null))
            .WithTracing(tracing => tracing
                .AddSource(Instrumentation.ActivitySource.Name)
                .AddProcessor(new EnrichSpanProcessor())
                .AddAspNetCoreInstrumentation()
                .AddEntityFrameworkCoreInstrumentation()
                .AddOtlpExporter())
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddMeter(Instrumentation.Meter.Name)
                .AddOtlpExporter());
    }
}
