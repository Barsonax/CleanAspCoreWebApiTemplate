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
        _api.SeedData(context =>
        {
            context.Jobs.Add(new Job()
            {
                Id = 0,
                Name = "Foo",
            });
        });

        //Act
        var result = await _api.CreateClient().GetFromJsonAsync<JobDto[]>("Job");

        //Assert
        result.Should().BeEquivalentTo(new[]
        {
            new JobDto()
            {
                Id = 0,
                Name = "Foo",
            }
        });
    }
}