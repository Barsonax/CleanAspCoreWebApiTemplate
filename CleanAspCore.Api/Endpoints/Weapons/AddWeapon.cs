using System.Text.Json.Serialization;
using CleanAspCore.Core.Common.NullableValidation;
using CleanAspCore.Core.Data.Models.Weapons;
using CleanAspCore.Data;

namespace CleanAspCore.Api.Endpoints.Weapons;

/// <summary>
/// Just here to support the derived types serialization.
/// </summary>
[JsonDerivedType(typeof(CreateBowRequest), typeDiscriminator: "bow")]
[JsonDerivedType(typeof(CreateSwordRequest), typeDiscriminator: "sword")]
public abstract class CreateWeaponRequest;

/// <summary>
/// Creates a new bow.
/// </summary>
public sealed class CreateBowRequest : CreateWeaponRequest
{
    /// <summary>
    /// The range of this bow.
    /// </summary>
    public required float Range { get; init; }

    /// <summary>
    /// The damage the bow does per shot
    /// </summary>
    public required float Damage { get; init; }

    /// <summary>
    /// Amount of shots per second.
    /// </summary>
    public required float RateOfFire { get; init; }
}

/// <summary>
/// Creates a new sword.
/// </summary>
public sealed class CreateSwordRequest : CreateWeaponRequest
{
    /// <summary>
    /// The damage the sword does per attack.
    /// </summary>
    public required float Damage { get; init; }

    /// <summary>
    /// Amount of attacks per second.
    /// </summary>
    public required float RateOfFire { get; init; }
}

internal sealed class CreateSwordResponseValidator : AbstractValidator<CreateSwordRequest>
{
    public CreateSwordResponseValidator()
    {
        this.ValidateNullableReferences();
    }
}

internal sealed class CreateBowRequestValidator : AbstractValidator<CreateBowRequest>
{
    public CreateBowRequestValidator()
    {
        this.ValidateNullableReferences();
    }
}

internal static class AddWeapon
{
    internal static async Task<CreatedAtRoute> Handle([FromBody] CreateWeaponRequest createWeaponRequest, HrContext context, CancellationToken cancellationToken)
    {
        var weapon = createWeaponRequest.ToDataModel();

        context.Weapons.AddRange(weapon);
        await context.SaveChangesAsync(cancellationToken);

        return TypedResults.CreatedAtRoute(nameof(GetWeaponById), new { weapon.Id });
    }

    private static Weapon ToDataModel(this CreateWeaponRequest request) => request switch
    {
        CreateBowRequest bow => new Bow
        {
            Damage = bow.Damage,
            Range = bow.Range,
            RateOfFire = bow.RateOfFire,
            Type = "bow"
        },
        CreateSwordRequest sword => new Sword
        {
            Damage = sword.Damage,
            RateOfFire = sword.RateOfFire,
            Type = "sword"
        },
        _ => throw new ArgumentOutOfRangeException(nameof(request), request, null)
    };
}
