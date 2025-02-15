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
        await response.AssertStatusCode(HttpStatusCode.Created);
        var createdId = response.GetGuidFromLocationHeader();
        sut.AssertDatabase(context =>
        {
            context.Jobs.Should().BeEquivalentTo([new { Id = createdId }]);
        });
    }
}
