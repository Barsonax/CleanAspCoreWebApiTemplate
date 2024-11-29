﻿using Microsoft.Extensions.Logging;

namespace CleanAspCore.Api.TestUtils.Logging;

public static class LoggingBuilderExtensions
{
    public static ILoggingBuilder AddNunitLogging(this ILoggingBuilder services)
    {
#pragma warning disable CA2000
        services.AddProvider(new NunitLoggerProvider());
#pragma warning restore CA2000
        return services;
    }
}
