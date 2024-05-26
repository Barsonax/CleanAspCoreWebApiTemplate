using System.Reflection;
using CleanAspCore;
using CleanAspCore.Data;
using CleanAspCore.Telemetry;

var builder = WebApplication.CreateBuilder(args);

builder.AddOpenApiServices();
builder.AddAuthServices();
builder.AddAppServices();
builder.AddOpenTelemetryServices();

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
    public partial class Program
    {
    }
}
