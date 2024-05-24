using System.Reflection;
using CleanAspCore;
using CleanAspCore.Data;
using CleanAspCore.Telemetry;

var builder = WebApplication.CreateBuilder(args);

builder.AddSwaggerServices();
builder.AddAuthServices();
builder.AddAppServices();
builder.AddOpenTelemetryServices();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);
builder.Services.AddDbContext<HrContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    var watchIteration = app.Configuration.GetValue("DOTNET_WATCH_ITERATION", 1);
    if (watchIteration == 1)
    {
        app.EnsureHrContextDatabaseIsCreated();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.AddAppRoutes();

app.Run();

namespace CleanAspCore
{
    public partial class Program
    {
    }
}
