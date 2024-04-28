using CleanAspCore.Features.Departments;
using CleanAspCore.Features.Departments.Endpoints;
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
        var response = await Sut.CreateClientFor<IDepartmentApiClient>().GetDepartmentById(department.Id);


        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
        var departmentDto = await response.Content.ReadFromJsonAsync<DepartmentDto>();
        departmentDto.Should().BeEquivalentTo(new
        {
            Id = department.Id
        });
    }

    [Test]
    public async Task CreateDepartment_IsAdded()
    {
        //Arrange
        var department = new CreateDepartmentRequestFaker().Generate();

        //Act
        var response = await Sut.CreateClientFor<IDepartmentApiClient>().CreateDepartment(department);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.Created);
        var createdId = response.GetGuidFromLocationHeader();
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
