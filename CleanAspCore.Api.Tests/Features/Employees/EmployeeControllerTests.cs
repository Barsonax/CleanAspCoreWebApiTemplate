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
            employee.ToDto()
        });
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

        var newEmployee = employee.ToDto();

        //Act
        var result = await _api.CreateClient().PostAsJsonAsync("Employee", newEmployee);

        //Assert
        result.EnsureSuccessStatusCode();
        _api.AssertDatabase(context =>
        {
            context.Employees.Should().BeEquivalentTo(new[]
            {
                newEmployee.ToDomain()
            });
        });
    }

    [Fact]
    public async Task UpdateEmployee_IsUpdated()
    {
        //Arrange
        var employee = Fakers.CreateEmployeeFaker().Generate();
        _api.SeedData(context =>
        {
            context.Employees.Add(employee);
        });
        
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
            }, c => c.ComparingByMembers<Employee>());
        });
    }

    [Fact]
    public async Task DeleteEmployee_IsDeleted()
    {
        //Arrange
        var employee = Fakers.CreateEmployeeFaker().Generate();
        _api.SeedData(context =>
        {
            context.Employees.Add(employee);
        });

        //Act
        var result = await _api.CreateClient().DeleteAsync($"Employee/{employee.Id}");

        //Assert
        result.EnsureSuccessStatusCode();
        _api.AssertDatabase(context => { context.Employees.Should().BeEmpty(); });
    }
}