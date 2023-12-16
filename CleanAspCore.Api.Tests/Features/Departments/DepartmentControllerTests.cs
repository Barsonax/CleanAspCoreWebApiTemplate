using CleanAspCore.Domain;
using CleanAspCore.Domain.Departments;
using CleanAspCore.Features.Import;

namespace CleanAspCore.Api.Tests.Features.Departments;

public class DepartmentControllerTests : TestBase
{
    [Test]
    public async Task SearchDepartments_ReturnsExpectedDepartments()
    {
        //Arrange
        var department = Fakers.CreateDepartmentFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Departments.Add(department);
        });

        //Act
        var result = await Sut.CreateClient().GetFromJsonAsync<DepartmentDto[]>("Department");

        //Assert
        result.Should().BeEquivalentTo(new[]
        {
            department
        }, c => c.ComparingByMembers<Entity>().ExcludingMissingMembers());
    }
}
