using System.Net;
using CleanAspCore.Domain;
using CleanAspCore.Domain.Departments;
using CleanAspCore.Features.Import;

namespace CleanAspCore.Api.Tests.Features.Departments;

public class DepartmentControllerTests : TestBase
{
    [Test]
    public async Task GetDepartmentById_ReturnsExpectedDepartment()
    {
        //Arrange
        var department = new DepartmentFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Departments.Add(department);
        });

        //Act
        var result = await Sut.CreateClient().GetFromJsonAsync<DepartmentDto>($"departments/{department.Id}");

        //Assert
        result.Should().BeEquivalentTo(department.ToDto());
    }

    [Test]
    public async Task AddDepartment_IsAdded()
    {
        //Arrange
        var department = new DepartmentFaker().Generate();

        //Act
        var response = await Sut.CreateClient().PostAsJsonAsync("departments", department.ToDto());
        await response.AssertStatusCode(HttpStatusCode.Created);
        var createdId = response.GetGuidFromLocationHeader();

        //Assert
        Sut.AssertDatabase(context =>
        {
            context.Departments.Should().BeEquivalentTo(new[]
            {
                new
                {
                    Id = createdId
                }
            });
        });
    }
}
