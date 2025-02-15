using CleanAspCore.Api.Tests.Fakers;
using FluentAssertions;

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
        sut.AssertDatabase(context =>
        {
            context.Jobs.Should().BeEquivalentTo([new { Id = createdId }]);
        });
    }
}
