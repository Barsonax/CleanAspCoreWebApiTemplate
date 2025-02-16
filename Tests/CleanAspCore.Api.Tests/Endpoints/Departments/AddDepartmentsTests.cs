using CleanAspCore.Api.Tests.Fakers;

namespace CleanAspCore.Api.Tests.Endpoints.Departments;

internal sealed class AddDepartmentsTests(TestWebApi sut)
{
    [Test]
    public async Task CreateDepartment_IsAddedToDatabase()
    {
        //Arrange
        var department = new CreateDepartmentRequestFaker().Generate();

        //Act
        var response = await sut.CreateClientFor<IDepartmentApiClient>().CreateDepartment(department);

        //Assert
        var createdId = response.GetGuidFromLocationHeader();
        await sut.AssertDatabase(async context =>
        {
            await Verify(context.Departments).AddNamedGuid(createdId, "DepartmentId");
        });
    }

    [Test]
    public async Task CreateDepartment_ReturnsResponse()
    {
        //Arrange
        var department = new CreateDepartmentRequestFaker().Generate();

        //Act
        var response = await sut.CreateClientFor<IDepartmentApiClient>().CreateDepartment(department);

        var createdId = response.GetGuidFromLocationHeader();

        //Assert
        await Verify(response)
            .IgnoreMember("RequestMessage")
            .AddNamedGuid(createdId, "DepartmentId")
            .ScrubInlineGuids();
    }
}
