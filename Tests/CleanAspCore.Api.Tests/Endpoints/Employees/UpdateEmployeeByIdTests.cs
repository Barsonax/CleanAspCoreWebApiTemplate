﻿using CleanAspCore.Api.Endpoints.Employees;
using FluentAssertions;

namespace CleanAspCore.Api.Tests.Endpoints.Employees;

internal sealed class UpdateEmployeeByIdTests(TestWebApi sut)
{
    [Test]
    public async Task UpdateEmployeeById_IsUpdated()
    {
        //Arrange
        var employee = new EmployeeFaker().Generate();
        sut.SeedData(context => { context.Employees.Add(employee); });

        UpdateEmployeeRequest updateEmployeeRequest = new() { FirstName = "Updated" };

        //Act
        var response = await sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteRole).UpdateEmployeeById(employee.Id, updateEmployeeRequest);

        //Assert
        await Assert.That(response).HasStatusCode(HttpStatusCode.NoContent);
        sut.AssertDatabase(context =>
        {
            context.Employees.Should().BeEquivalentTo([
                new
                {
                    FirstName = "Updated",
                    LastName = employee.LastName,
                }
            ]);
        });
    }

    [Test]
    public async Task UpdateEmployeeById_DoesNotExist_ReturnsNotFound()
    {
        //Arrange
        var employee = new EmployeeFaker().Generate();

        UpdateEmployeeRequest updateEmployeeRequest = new() { FirstName = "Updated" };

        //Act
        var response = await sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteRole).UpdateEmployeeById(employee.Id, updateEmployeeRequest);

        //Assert
        await Assert.That(response).HasStatusCode(HttpStatusCode.NotFound);
    }
}
