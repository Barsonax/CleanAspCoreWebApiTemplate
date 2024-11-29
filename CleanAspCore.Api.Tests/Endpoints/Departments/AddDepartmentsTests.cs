namespace CleanAspCore.Api.Tests.Endpoints.Departments;

internal sealed class AddDepartmentsTests : TestBase
{
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
            context.Departments.Should().BeEquivalentTo(new[] { new { Id = createdId } });
        });
    }
}
