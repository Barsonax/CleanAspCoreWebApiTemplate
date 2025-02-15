using CleanAspCore.Api.Tests.Fakers;

namespace CleanAspCore.Api.Tests.Endpoints.Jobs;

internal sealed class CreateJobTests(TestWebApi sut)
{
    [Test]
    public async Task CreateJob_IsAdded()
    {
        //Arrange
        var createJobRequest = new CreateJobRequestFaker().Generate();

        //Act
        var response = await sut.CreateClientFor<IJobApiClient>().CreateJob(createJobRequest);

        //Assert
        await Assert.That(response).HasStatusCode(HttpStatusCode.Created);
        var createdId = response.GetGuidFromLocationHeader();
        await sut.AssertDatabase(async context =>
        {
            await Assert.That(context.Jobs)
                .HasCount().EqualTo(1).And
                .Contains(x => x.Id == createdId);
        });
    }
}
