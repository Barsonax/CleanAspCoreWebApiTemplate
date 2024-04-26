using System.Reflection;
using System.Runtime.CompilerServices;
using CleanAspCore;
using CleanAspCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;


[assembly: InternalsVisibleTo("CleanAspCore.Api.Tests")]

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<HrContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MigrateHrContext();
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

public partial class Program { }
