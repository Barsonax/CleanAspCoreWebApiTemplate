using System.Text.Json;
using CleanAspCore.Api.Endpoints.Weapons;
using CleanAspCore.Api.Tests.Fakers;

namespace CleanAspCore.Api.Tests.Endpoints.Weapons;

internal sealed class CreateWeaponTests(TestWebApi sut)
{
    [Test]
    public async Task CreateSword_IsAdded()
    {
        //Arrange
        var request = new CreateSwordRequestFaker().Generate();

        //Act
        var response = await sut.CreateUntypedClientFor().PostAsJsonAsync<ICreateWeaponRequest>("/weapons", request);

        //Assert
        await Assert.That(response).HasStatusCode(HttpStatusCode.Created);
        var createdId = response.GetGuidFromLocationHeader();
        await sut.AssertDatabase(async context =>
        {
            await Assert.That(context.Weapons)
                .HasCount().EqualTo(1).And
                .Contains(x => x.Id == createdId);
        });
    }
}
