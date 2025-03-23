namespace CleanAspCore.Api.Tests.Endpoints;

public class HealthCheckTests(TestWebApi sut)
{
    [Test]
    public async Task GetHealth_Returns200()
    {
        //Act
        var response = await sut.CreateUntypedClientFor().GetAsync("/health");

        //Assert
        await Assert.That(response)
            .HasStatusCode(HttpStatusCode.OK);
    }

    [Test]
    public async Task GetAlive_Returns200()
    {
        //Act
        var response = await sut.CreateUntypedClientFor().GetAsync("/alive");

        //Assert
        await Assert.That(response)
            .HasStatusCode(HttpStatusCode.OK);
    }
}
