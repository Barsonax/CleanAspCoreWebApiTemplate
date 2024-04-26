using System.Net;
using System.Web;
using CleanAspCore.Domain;
using CleanAspCore.Domain.Employees;
using CleanAspCore.Features.Employees;
using CleanAspCore.Features.Import;

namespace CleanAspCore.Api.Tests.Features.Employees;

public class EmployeeControllerTests : TestBase
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
        var result = await Sut.CreateClient().GetFromJsonAsync<EmployeeDto>($"employees/{employee.Id}");

        //Assert
        result.Should().BeEquivalentTo(employee.ToDto());
    }

    [Test]
    public async Task AddEmployee_IsAdded()
    {
        //Arrange
        var employee = new EmployeeFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Departments.Add(employee.Department!);
            context.Jobs.Add(employee.Job!);
        });

        //Act
        var response = await Sut.CreateClient().PostAsJsonAsync("employees", employee.ToDto());
        await response.AssertStatusCode(HttpStatusCode.Created);
        var createdId = response.GetGuidFromLocationHeader();

        //Assert
        Sut.AssertDatabase(context =>
        {
            context.Employees.Should().BeEquivalentTo(new[]
            {
                new
                {
                    Id = createdId
                }
            });
        });
    }

    [Test]
    public async Task UpdateEmployee_IsUpdated()
    {
        //Arrange
        var employee = new EmployeeFaker().Generate();
        Sut.SeedData(context => { context.Employees.Add(employee); });

        UpdateEmployeeRequest updateEmployeeRequest = new()
        {
            FirstName = "Updated"
        };

        //Act
        var result = await Sut.CreateClient().PutAsJsonAsync($"employees/{employee.Id}", updateEmployeeRequest);

        //Assert
        await result.AssertStatusCode(HttpStatusCode.NoContent);
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
    public async Task DeleteEmployee_IsDeleted()
    {
        //Arrange
        var employee = new EmployeeFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Employees.Add(employee);
        });

        //Act
        var result = await Sut.CreateClient().DeleteAsync($"employees/{employee.Id}");

        //Assert
        result.EnsureSuccessStatusCode();
        Sut.AssertDatabase(context => { context.Employees.Should().BeEmpty(); });
    }
}
