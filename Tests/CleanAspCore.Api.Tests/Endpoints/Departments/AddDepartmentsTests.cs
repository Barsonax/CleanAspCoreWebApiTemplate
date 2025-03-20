using CleanAspCore.Api.Tests.Fakers;

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
        await sut.AssertDatabase(async context =>
        {
            await Assert.That(context.Departments)
                .HasCount().EqualTo(1).And
                .Contains(x => x.Id == createdId);
        });
    }

    [Test]
    public async Task CreateDepartment_MissingProperties_ReturnsBadRequestWithDetails()
    {
        //Act
        var response = await sut.CreateUntypedClientFor().PostAsJsonAsync("departments", new { });

        //Assert
        await Assert.That(response).HasBadRequest("name", "city");
    }

    [Test]
    public async Task CreateDepartment_InvalidJson_ReturnsBadRequestWithDetails()
    {
        //Act
        var response = await sut.CreateUntypedClientFor().PostAsJsonAsync("departments", "{/}");

        //Assert
        await Assert.That(response).HasBadRequest();
        var responseText = await response.Content.ReadAsStringAsync();
        await Assert.That(responseText).Contains("The JSON value could not be converted to");
    }
}
