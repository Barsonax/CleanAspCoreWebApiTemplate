using CleanAspCore.Api.Endpoints.Weapons;
using Refit;

namespace CleanAspCore.Api.Tests.Endpoints.Weapons;

internal interface IWeaponApiClient
{
    [Get("/weapons/{id}")]
    Task<HttpResponseMessage> GetWeaponById(Guid id);

    [Post("/weapons")]
    Task<HttpResponseMessage> CreateWeapon(ICreateWeaponRequest createJobRequest);
}
