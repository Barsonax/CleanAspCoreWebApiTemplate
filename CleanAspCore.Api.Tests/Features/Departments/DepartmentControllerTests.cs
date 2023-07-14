using CleanAspCore.Domain.Departments;

namespace CleanAspCore.Api.Tests.Features.Departments;

public class DepartmentControllerTests
{
    private readonly TestWebApplicationFactory _api;

    public DepartmentControllerTests(TestWebApplicationFactory api)
    {
        _api = api;
    }
    
    [Fact]
    public async Task SearchDepartments_ReturnsExpectedDepartments()
    {
        //Arrange
        _api.SeedData(context =>
        {
            context.Departments.Add(new Department()
            {
                Id = 0,
                Name = "Foo",
                City = "bar",
            });
        });
        
        //Act
        var result = await _api.CreateClient().GetFromJsonAsync<DepartmentDto[]>("Department");

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
}