using CleanAspCore.Api.TestUtils.Fakers;
using CleanAspCore.Endpoints.Employees;

namespace CleanAspCore.Api.Tests.Endpoints.Employees;

internal sealed class UpdateEmployeeByIdTests : TestBase
{
    [Test]
    public async Task UpdateEmployeeById_IsUpdated()
    {
        //Arrange
        var employee = new EmployeeFaker().Generate();
        Sut.SeedData(context => { context.Employees.Add(employee); });

        UpdateEmployeeRequest updateEmployeeRequest = new() { FirstName = "Updated" };

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteRole).UpdateEmployeeById(employee.Id, updateEmployeeRequest);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.NoContent);
        Sut.AssertDatabase(context =>
        {
            context.Employees.Should().BeEquivalentTo(new[]
            {
                new
                {
                    FirstName = "Updated",
                    LastName = employee.LastName,
                }
            });
        });
    }

    [Test]
    public async Task UpdateEmployeeById_DoesNotExist_ReturnsNotFound()
    {
        //Arrange
        var employee = new EmployeeFaker().Generate();

        UpdateEmployeeRequest updateEmployeeRequest = new() { FirstName = "Updated" };

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteRole).UpdateEmployeeById(employee.Id, updateEmployeeRequest);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.NotFound);
    }
}
