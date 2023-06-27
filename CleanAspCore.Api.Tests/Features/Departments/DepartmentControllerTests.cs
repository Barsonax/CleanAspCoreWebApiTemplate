using System.Net.Http.Json;
using CleanAspCore.Api.Tests.Helpers;
using CleanAspCore.Domain.Department;
using FluentAssertions;

namespace CleanAspCore.Api.Tests.Features.Departments;

public class DepartmentControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task SearchDepartments_ReturnsExpectedDepartments()
    {
        //Arrange
        Context.Departments.Add(new Department()
        {
            Id = 0,
            Name = "Foo",
            City = "bar",
        });

        await Context.SaveChangesAsync();
        
        //Act
        var result = await Client.GetFromJsonAsync<DepartmentDto[]>("Department");

        //Assert
        result.Should().BeEquivalentTo(new[]
        {
            new DepartmentDto()
            {
                Id = 0,
                Name = "Foo",
                City = "bar",
            }
        });
    }
    
    public DepartmentControllerTests(PostgreSqlLifetime fixture) : base(fixture)
    {
    }
}