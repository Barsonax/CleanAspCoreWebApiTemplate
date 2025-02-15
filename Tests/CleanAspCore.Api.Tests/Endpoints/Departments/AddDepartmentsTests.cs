using CleanAspCore.Api.Tests.Fakers;
using FluentAssertions;

namespace CleanAspCore.Api.Tests.Endpoints.Departments;

internal sealed class AddDepartmentsTests(TestWebApi sut)
{
    [Test]
    public async Task CreateDepartment_IsAdded()
    {
        //Arrange
        var department = new CreateDepartmentRequestFaker().Generate();

        //Act
        var response = await sut.CreateClientFor<IDepartmentApiClient>().CreateDepartment(department);

        //Assert
        await Assert.That(response).HasStatusCode(HttpStatusCode.Created);

        var createdId = response.GetGuidFromLocationHeader();
        sut.AssertDatabase(context =>
        {
            context.Departments.Should().BeEquivalentTo([new { Id = createdId }]);
        });
    }
}
