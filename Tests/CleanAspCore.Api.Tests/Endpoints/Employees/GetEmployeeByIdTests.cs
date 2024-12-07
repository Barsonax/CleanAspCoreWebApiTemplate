﻿namespace CleanAspCore.Api.Tests.Endpoints.Employees;

internal sealed class GetEmployeeByIdTests(TestWebApi sut)
{
    [Test]
    public async Task GetEmployeeById_ReturnsExpectedEmployee()
    {
        //Arrange
        var employee = new EmployeeFaker().Generate();
        sut.SeedData(context =>
        {
            context.Employees.Add(employee);
        });

        //Act
        var response = await sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.ReadRole).GetEmployeeById(employee.Id);

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
        var response = await sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.ReadRole).GetEmployeeById(employee.Id);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.NotFound);
    }
}
