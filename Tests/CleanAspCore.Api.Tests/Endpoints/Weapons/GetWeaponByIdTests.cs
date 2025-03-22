using CleanAspCore.Api.Endpoints.Weapons;
using CleanAspCore.TestUtils.Fakers;

namespace CleanAspCore.Api.Tests.Endpoints.Weapons;

internal sealed class GetWeaponByIdTests(TestWebApi sut)
{
    [Test]
    public async Task GetWeaponById_Sword_ReturnsExpectedWeapon()
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
        await Assert.That(response)
            .IsTypeOf<GetSwordResponse>()
            .HasMember(x => x!.Id).EqualTo(weapon.Id);
    }

    [Test]
    public async Task GetWeaponById_Bow_ReturnsExpectedWeapon()
    {
        //Arrange
        var weapon = new BowFaker().Generate();
        sut.SeedData(context =>
        {
            context.Weapons.Add(weapon);
        });

        //Act
        var response = await sut.CreateUntypedClientFor().GetFromJsonAsync<IWeaponResponse>($"weapons/{weapon.Id}");

        //Assert
        await Assert.That(response)
            .IsTypeOf<GetBowResponse>()
            .HasMember(x => x!.Id).EqualTo(weapon.Id);
    }
}
