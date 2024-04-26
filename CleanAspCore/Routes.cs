﻿using CleanAspCore.Features.Departments;
using CleanAspCore.Features.Employees;
using CleanAspCore.Features.Import;
using CleanAspCore.Features.Jobs;

namespace CleanAspCore;

public static class EndpointRouteBuilderExtensions
{
    public static void AddRoutes(this IEndpointRouteBuilder host)
    {
        var departmentGroup = host.MapGroup("/departments");
        departmentGroup.MapPost("/", AddDepartments.Handle);
        departmentGroup.MapGet("/{id:guid}", GetDepartmentById.Handle)
            .WithName(nameof(GetDepartmentById));

        var employeeGroup = host.MapGroup("/employees");
        employeeGroup.MapPost("/", AddEmployee.Handle);
        employeeGroup.MapGet("/{id:guid}", GetEmployeeById.Handle)
            .WithName(nameof(GetEmployeeById));
        employeeGroup.MapDelete("/{id:guid}", DeleteEmployeeById.Handle);
        employeeGroup.MapPut("/{id:guid}", UpdateEmployeeById.Handle);

        var jobGroup = host.MapGroup("/jobs");
        jobGroup.MapPost("/",AddJobs.Handle);
        jobGroup.MapGet("/{id:guid}", GetJobById.Handle)
            .WithName(nameof(GetJobById));

        var importGroup = host.MapGroup("/import");
        importGroup.MapPut("/", ImportTestData.Handle);
    }
}