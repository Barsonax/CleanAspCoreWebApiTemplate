﻿using System.Diagnostics;
using OpenTelemetry;

namespace CleanAspCore.Telemetry;

public class EnrichSpanProcessor : BaseProcessor<Activity>
{
    public override void OnEnd(Activity data)
    {
        foreach (var baggage in data.Baggage)
        {
            if (baggage.Value == null) continue;
            data.SetTag(baggage.Key, baggage.Value);
        }
    }
}
