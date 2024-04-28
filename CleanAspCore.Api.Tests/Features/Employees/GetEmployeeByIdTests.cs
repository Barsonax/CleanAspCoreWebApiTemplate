using CleanAspCore.Features.Employees;
using CleanAspCore.Features.Employees.Endpoints;
using CleanAspCore.Features.Import;

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
        var response = await Sut.CreateClientFor<IEmployeeApiClient>().GetEmployeeById(employee.Id);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
        var employeeDto = await response.Content.ReadFromJsonAsync<EmployeeDto>();
        employeeDto.Should().BeEquivalentTo(new { Id = employee.Id });
    }
}
