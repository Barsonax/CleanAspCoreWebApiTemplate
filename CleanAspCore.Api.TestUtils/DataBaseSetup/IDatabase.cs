﻿using Microsoft.Extensions.Hosting;

namespace CleanAspCore.Api.TestUtils.DataBaseSetup;

public interface IDatabase
{
    string ConnectionString { get; }

    public void EnsureInitialized(IHost host);
    public Task Clean();
}
