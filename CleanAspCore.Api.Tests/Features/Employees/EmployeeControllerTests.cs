using CleanAspCore.Domain.Departments;
using CleanAspCore.Domain.Employees;
using CleanAspCore.Domain.Jobs;

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
        _api.SeedData(context =>
        {
            context.Employees.Add(new Employee()
            {
                Id = 0,
                FirstName = "Foo",
                LastName = "Bar",
                Email = "email@foobar.com",
                Gender = "Weird",
                DepartmentId = 1,
                JobId = 2
            });
        
            context.Departments.Add(new Department()
            {
                Id = 1,
                Name = "Foo",
                City = "Bar"
            });
        
            context.Jobs.Add(new Job
            {
                Id = 2,
                Name = "Foo",
            });
        });
        
        //Act
        var result = await _api.CreateClient().GetFromJsonAsync<EmployeeDto[]>("Employee");

        //Assert
        result.Should().BeEquivalentTo(new[]
        {
            new EmployeeDto()
            {
                Id = 0,
                FirstName = "Foo",
                LastName = "Bar",
                Email = "email@foobar.com",
                Gender = "Weird",
                DepartmentId = 1,
                JobId = 2
            }
        });
    }
    
    [Fact]
    public async Task AddEmployee_IsAdded()
    {
        //Arrange
        _api.SeedData(context =>
        {
            context.Departments.Add(new Department()
            {
                Id = 1,
                Name = "Foo",
                City = "Bar"
            });
        
            context.Jobs.Add(new Job()
            {
                Id = 2,
                Name = "Foo",
            });
        });

        var newEmployee = new EmployeeDto()
        {
            Id = 0,
            FirstName = "Foo",
            LastName = "Bar",
            Email = "email@foobar.com",
            Gender = "Weird",
            DepartmentId = 1,
            JobId = 2
        };
        
        //Act
        var result = await _api.CreateClient().PostAsJsonAsync("Employee", newEmployee);
        result.EnsureSuccessStatusCode();

        //Assert
        _api.AssertDatabase(context =>
        {
            context.Employees
                .Include(x => x.Department)
                .Include(x => x.Job)
                .Should().BeEquivalentTo(new[]
            {
                new Employee()
                {
                    Id = 0,
                    FirstName = "Foo",
                    LastName = "Bar",
                    Email = "email@foobar.com",
                    Gender = "Weird",
                    DepartmentId = 1,
                    Department = new Department()
                    {
                        Id = 1,
                        Name = "Foo",
                        City = "Bar"
                    },
                    JobId = 2,
                    Job = new Job()
                    {
                        Id = 2,
                        Name = "Foo",
                    }
                }
            });
        });
    }
}