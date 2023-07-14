using CleanAspCore.Domain.Departments;
using Xunit.Abstractions;

namespace CleanAspCore.Api.Tests.Features.Departments;

public class DepartmentControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task SearchDepartments_ReturnsExpectedDepartments()
    {
        //Arrange
        await using var api = CreateApi();
        api.SeedData(context =>
        {
            context.Departments.Add(new Department()
            {
                Id = 0,
                Name = "Foo",
                City = "bar",
            });
        });
        
        //Act
        var result = await api.CreateClient().GetFromJsonAsync<DepartmentDto[]>("Department");

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

    public DepartmentControllerTests(PostgreSqlLifetime fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }
}