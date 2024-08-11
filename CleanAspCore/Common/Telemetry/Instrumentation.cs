using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace CleanAspCore.Common.Telemetry;

internal static class Instrumentation
{
    internal static readonly Meter Meter = new("CleanAspCore", "1.0.0");
    internal static readonly ActivitySource ActivitySource = new("CleanAspCore", "1.0.0");
}
