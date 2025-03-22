using CleanAspCore.Core.Common.GenericValidation;

namespace CleanAspCore.Api.Endpoints.Weapons;

internal static class WeaponEndpointConfig
{
    internal static void AddWeaponsRoutes(this IEndpointRouteBuilder host)
    {
        var weaponGroup = host
            .MapGroup("/weapons")
            .WithTags("Weapons");

        weaponGroup.MapPost("/", AddWeapon.Handle)
            .WithRequestBodyValidation();

        weaponGroup.MapGet("/{id:guid}", GetWeaponById.Handle)
            .WithName(nameof(GetWeaponById));
    }
}
