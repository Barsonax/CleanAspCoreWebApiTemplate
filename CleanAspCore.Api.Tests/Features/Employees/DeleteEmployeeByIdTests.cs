using CleanAspCore.Data.Models;
using CleanAspCore.Features.Employees;
using CleanAspCore.Features.Import;

namespace CleanAspCore.Api.Tests.Features.Employees;

public class DeleteEmployeeByIdTests : TestBase
{
    [Test]
    public async Task DeleteEmployeeById_IsDeleted()
    {
        //Arrange
        var employee = new EmployeeFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Employees.Add(employee);
        });

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>().DeleteEmployeeById(employee.Id);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.NoContent);
        Sut.AssertDatabase(context => { context.Employees.Should().BeEmpty(); });
    }

    [Test]
    public async Task DeleteEmployeeById_DoesNotExist_ReturnsNotFound()
    {
        //Arrange
        var id = EmployeeId.CreateNew();

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>().DeleteEmployeeById(id);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.NotFound);
    }
}
