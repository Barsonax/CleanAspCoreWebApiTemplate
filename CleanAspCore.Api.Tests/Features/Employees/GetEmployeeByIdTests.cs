﻿using CleanAspCore.Api.Tests.Fakers;

namespace CleanAspCore.Api.Tests.Features.Employees;

public class GetEmployeeByIdTests : TestBase
{
    [Test]
    public async Task GetEmployeeById_ReturnsExpectedEmployee()
    {
        //Arrange
        var employee = new EmployeeFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Employees.Add(employee);
        });

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.ReadEmployeesRole).GetEmployeeById(employee.Id);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
        await response.AssertJsonBodyIsEquivalentTo(new { Id = employee.Id });
    }

    [Test]
    public async Task GetEmployeeById_DoesNotExist_ReturnsNotFound()
    {
        //Arrange
        var employee = new EmployeeFaker().Generate();

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.ReadEmployeesRole).GetEmployeeById(employee.Id);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.NotFound);
    }
}
