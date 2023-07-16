using CleanAspCore.Domain;
using CleanAspCore.Domain.Employees;

namespace CleanAspCore.Api.Tests.Features.Employees;

public class EmployeeControllerTests
{
    private readonly TestWebApi _api;

    public EmployeeControllerTests(TestWebApi api)
    {
        _api = api;
    }

    [Fact]
    public async Task SearchEmployee_ReturnsExpectedJobs()
    {
        //Arrange
        var employee = Fakers.CreateEmployeeFaker().Generate();
        _api.SeedData(context => { context.Employees.Add(employee); });

        //Act
        var result = await _api.CreateClient().GetFromJsonAsync<EmployeeDto[]>("Employee");

        //Assert
        result.Should().BeEquivalentTo(new[]
        {
            employee
        }, c => c.ComparingByMembers<Entity>().ExcludingMissingMembers());
    }

    [Fact]
    public async Task AddEmployee_IsAdded()
    {
        //Arrange
        var employee = Fakers.CreateEmployeeFaker().Generate();
        _api.SeedData(context =>
        {
            context.Departments.Add(employee.Department);
            context.Jobs.Add(employee.Job);
        });
        
        //Act
        var result = await _api.CreateClient().PostAsJsonAsync("Employee", employee.ToDto());

        //Assert
        result.EnsureSuccessStatusCode();
        _api.AssertDatabase(context =>
        {
            context.Employees.Should().BeEquivalentTo(new[]
            {
                employee
            });
        });
    }

    [Fact]
    public async Task UpdateEmployee_IsUpdated()
    {
        //Arrange
        var employee = Fakers.CreateEmployeeFaker().Generate();
        _api.SeedData(context => { context.Employees.Add(employee); });

        var updatedEmployee = employee.ToDto() with
        {
            FirstName = "Updated",
            LastName = "Updated"
        };

        //Act
        var result = await _api.CreateClient().PutAsJsonAsync("Employee", updatedEmployee);

        //Assert
        result.EnsureSuccessStatusCode();
        _api.AssertDatabase(context =>
        {
            context.Employees.Should().BeEquivalentTo(new[]
            {
                updatedEmployee.ToDomain()
            });
        });
    }

    [Fact]
    public async Task DeleteEmployee_IsDeleted()
    {
        //Arrange
        var employee = Fakers.CreateEmployeeFaker().Generate();
        _api.SeedData(context => { context.Employees.Add(employee); });

        //Act
        var result = await _api.CreateClient().DeleteAsync($"Employee/{employee.Id}");

        //Assert
        result.EnsureSuccessStatusCode();
        _api.AssertDatabase(context => { context.Employees.Should().BeEmpty(); });
    }
}