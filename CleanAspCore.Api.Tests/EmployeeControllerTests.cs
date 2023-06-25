using System.Net.Http.Json;
using CleanAspCore.Api.Tests.Helpers;
using CleanAspCore.Domain;
using FluentAssertions;

namespace CleanAspCore.Api.Tests;

public class EmployeeControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task SearchEmployee_ReturnsExpectedJobs()
    {
        //Arrange
        Context.Employees.Add(new Employee()
        {
            Id = 0,
            FirstName = "Foo",
            LastName = "Bar",
            Email = "email",
            Gender = "Weird",
            DepartmentId = 1,
            JobId = 2
        });
        
        Context.Departments.Add(new Department()
        {
            Id = 1,
            Name = "Foo",
            City = "Bar"
        });
        
        Context.Jobs.Add(new Job()
        {
            Id = 2,
            Name = "Foo",
        });

        await Context.SaveChangesAsync();
        
        //Act
        var result = await Client.GetFromJsonAsync<EmployeeDto[]>("Employee");

        //Assert
        result.Should().BeEquivalentTo(new[]
        {
            new EmployeeDto()
            {
                Id = 0,
                FirstName = "Foo",
                LastName = "Bar",
                Email = "email",
                Gender = "Weird",
                DepartmentId = 1,
                JobId = 2
            }
        });
    }
    
    public EmployeeControllerTests(PostgreSqlLifetime fixture) : base(fixture)
    {
    }
}