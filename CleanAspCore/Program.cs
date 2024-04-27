using System.Reflection;
using System.Runtime.CompilerServices;
using CleanAspCore;
using CleanAspCore.Data;

[assembly: InternalsVisibleTo("CleanAspCore.Api.Tests")]

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SupportNonNullableReferenceTypes();
});
builder.Services.AddAuthorization();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<HrContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.EnsureHrContextDatabaseIsCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.AddRoutes();

app.Run();

namespace CleanAspCore
{
    public partial class Program { }
}
