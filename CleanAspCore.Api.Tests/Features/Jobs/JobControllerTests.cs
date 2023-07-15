using CleanAspCore.Domain.Jobs;

namespace CleanAspCore.Api.Tests.Features.Jobs;

public class JobControllerTests
{
    private readonly TestWebApi _api;

    public JobControllerTests(TestWebApi api)
    {
        _api = api;
    }


    [Fact]
    public async Task SearchJobs_ReturnsExpectedJobs()
    {
        //Arrange
        var job = Fakers.CreateJobFaker().Generate();
        _api.SeedData(context => { context.Jobs.Add(job); });

        //Act
        var result = await _api.CreateClient().GetFromJsonAsync<JobDto[]>("Job");

        //Assert
        result.Should().BeEquivalentTo(new[]
        {
            job.ToDto()
        });
    }
}