using CleanAspCore.Domain;
using CleanAspCore.Domain.Departments;

namespace CleanAspCore.Api.Tests.Features.Departments;

public class DepartmentControllerTests
{
    private readonly TestWebApi _api;

    public DepartmentControllerTests(TestWebApi api)
    {
        _api = api;
    }

    [Fact]
    public async Task SearchDepartments_ReturnsExpectedDepartments()
    {
        //Arrange
        var department = Fakers.CreateDepartmentFaker().Generate();
        _api.SeedData(context => { context.Departments.Add(department); });

        //Act
        var result = await _api.CreateClient().GetFromJsonAsync<DepartmentDto[]>("Department");

        //Assert
        result.Should().BeEquivalentTo(new[]
        {
            department
        }, c => c.ComparingByMembers<Entity>().ExcludingMissingMembers());
    }
}