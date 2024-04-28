﻿using CleanAspCore.Features.Employees.Endpoints;

namespace CleanAspCore.Features.Employees;

internal static class Routes
{
    internal static void AddEmployeesRoutes(this IEndpointRouteBuilder host)
    {
        var employeeGroup = host
            .MapGroup("/employees")
            .WithTags("Employees");

        employeeGroup.MapPost("/", AddEmployee.Handle)
            .WithRequestValidation<CreateEmployeeRequest>();

        employeeGroup.MapGet("/{id:guid}", GetEmployeeById.Handle)
            .WithName(nameof(GetEmployeeById));

        employeeGroup.MapDelete("/{id:guid}", DeleteEmployeeById.Handle);

        employeeGroup.MapPut("/{id:guid}", UpdateEmployeeById.Handle);
    }
}
