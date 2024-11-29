using System.Reflection;
using CleanAspCore;
using CleanAspCore.Common.Telemetry;
using CleanAspCore.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddOpenApiServices();
builder.AddAuthServices();
builder.AddAppServices();
builder.AddOpenTelemetryServices();
builder.Services.AddHttpClient();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);
builder.Services.AddDbContext<HrContext>();

var app = builder.Build();

app.RunMigrations();

app.UseOpenApi();

app.UseAuthentication();
app.UseAuthorization();

app.AddAppRoutes();

app.Run();

namespace CleanAspCore
{
#pragma warning disable CA1515
    public partial class Program
#pragma warning restore CA1515
    {
    }
}
