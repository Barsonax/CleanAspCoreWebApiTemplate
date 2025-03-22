using CleanAspCore.Api.Endpoints.Weapons;
using CleanAspCore.TestUtils.Fakers;

namespace CleanAspCore.Api.Tests.Endpoints.Weapons;

internal sealed class GetWeaponByIdTests(TestWebApi sut)
{
    [Test]
    public async Task GetWeaponById_ReturnsExpectedWeapon()
    {
        //Arrange
        var weapon = new SwordFaker().Generate();
        sut.SeedData(context =>
        {
            context.Weapons.Add(weapon);
        });

        //Act
        var response = await sut.CreateUntypedClientFor().GetFromJsonAsync<IWeaponResponse>($"weapons/{weapon.Id}");

        //Assert
        await Assert.That(response).HasMember(x => x!.Id).EqualTo(weapon.Id);
    }
}
