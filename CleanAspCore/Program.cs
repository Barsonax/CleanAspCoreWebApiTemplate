using System.Reflection;
using CleanAspCore.Data;
using CleanAspCore.Features.Departments;
using CleanAspCore.Features.Employees;
using CleanAspCore.Features.Import;
using CleanAspCore.Features.Jobs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddImport();
builder.Services.AddDbContext<HrContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Transient;
});

var app = builder.Build();

using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
var context = serviceScope.ServiceProvider.GetRequiredService<HrContext>();
context.Database.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGetDepartments();
app.MapGetJobs();
app.MapGetEmployees();
app.MapAddEmployee();
app.MapUpdateEmployeeById();
app.MapDeleteEmployeeById();

app.Run();

namespace CleanAspCore
{
    public partial class Program { }
}