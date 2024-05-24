using System.Reflection;
using CleanAspCore;
using CleanAspCore.Data;

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
    app.EnsureHrContextDatabaseIsCreated();
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
