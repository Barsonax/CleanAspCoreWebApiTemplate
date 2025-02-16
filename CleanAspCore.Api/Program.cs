using System.Reflection;
using CleanAspCore.Api;
using CleanAspCore.Core.Common.Telemetry;
using CleanAspCore.Data;
using Microsoft.AspNetCore.Routing.Constraints;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.Configure<RouteOptions>(
    options => options.SetParameterPolicy<RegexInlineRouteConstraint>("regex"));

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

namespace CleanAspCore.Api
{
#pragma warning disable CA1515
    public partial class Program
#pragma warning restore CA1515
    {
    }
}
