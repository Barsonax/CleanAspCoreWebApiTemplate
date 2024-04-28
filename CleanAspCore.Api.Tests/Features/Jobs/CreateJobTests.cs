using CleanAspCore.Features.Import;
using CleanAspCore.Features.Jobs;
using CleanAspCore.Features.Jobs.Endpoints;

namespace CleanAspCore.Api.Tests.Features.Jobs;

public class CreateJobTests : TestBase
{
    [Test]
    public async Task CreateJob_IsAdded()
    {
        //Arrange
        var createJobRequest = new CreateJobRequestFaker().Generate();

        //Act
        var response = await Sut.CreateClientFor<IJobApiClient>().CreateJob(createJobRequest);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.Created);
        var createdId = response.GetGuidFromLocationHeader();
        Sut.AssertDatabase(context =>
        {
            context.Jobs.Should().BeEquivalentTo(new[]
            {
                new
                {
                    Id = createdId
                }
            });
        });
    }
}