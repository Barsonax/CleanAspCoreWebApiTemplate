using System.Diagnostics.CodeAnalysis;
using CleanAspCore.Api.TestDataGenerator;
using CleanAspCore.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: ExcludeFromCodeCoverage]

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<HrContext>();
builder.Services.AddHostedService<TestDataGeneratorService>();

IHost app = builder.Build();

app.EnsureHrContextDatabaseIsCreated();

await app.RunAsync();
