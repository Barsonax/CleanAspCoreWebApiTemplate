﻿using CleanAspCore.Api.Tests.Fakers;

namespace CleanAspCore.Api.Tests.Features.Employees;

public class GetEmployees : TestBase
{
    [Test]
    public async Task? GetEmployees_NoEmployees_ReturnsEmptyPage()
    {
        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.ReadEmployeesRole).GetEmployees(1);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
        await response.AssertJsonBodyIsEquivalentTo(new
        {
            TotalPages = 0,
            TotalRecords = 0,
            PageNumber = 0,
            Data = new List<Guid>()
        });
    }

    [Test]
    public async Task GetEmployees_FirstPage_ReturnsExpectedEmployees()
    {
        //Arrange
        var department = new DepartmentFaker().Generate();
        var job = new JobFaker().Generate();
        var employees = new EmployeeFaker()
            .RuleFor(x => x.Department, department)
            .RuleFor(x => x.Job, job)
            .Generate(15);
        Sut.SeedData(context =>
        {
            context.Employees.AddRange(employees);
        });

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.ReadEmployeesRole).GetEmployees(1);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
        await response.AssertJsonBodyIsEquivalentTo(new
        {
            TotalPages = 2,
            TotalRecords = 15,
            PageNumber = 1,
            Data = employees
                .OrderBy(x => x.FirstName)
                .Select(x => new { x.Id })
                .Take(10)
                .ToList()
        });
    }

    [Test]
    public async Task GetEmployees_SecondPage_ReturnsExpectedEmployees()
    {
        //Arrange
        var department = new DepartmentFaker().Generate();
        var job = new JobFaker().Generate();
        var employees = new EmployeeFaker()
            .RuleFor(x => x.Department, department)
            .RuleFor(x => x.Job, job)
            .Generate(15);
        Sut.SeedData(context =>
        {
            context.Employees.AddRange(employees);
        });

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.ReadEmployeesRole).GetEmployees(2);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
        await response.AssertJsonBodyIsEquivalentTo(new
        {
            TotalPages = 2,
            TotalRecords = 15,
            PageNumber = 2,
            Data = employees
                .OrderBy(x => x.FirstName)
                .Select(x => new { x.Id })
                .Skip(10)
                .Take(10)
                .ToList()
        });
    }
}
