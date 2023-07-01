using CleanAspCore.Domain.Jobs;

namespace CleanAspCore.Api.Tests.Features.Jobs;

public class JobControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task SearchJobs_ReturnsExpectedJobs()
    {
        //Arrange
        await using var api = CreateApi();
        api.SeedData(context =>
        {
            context.Jobs.Add(new Job()
            {
                Id = 0,
                Name = "Foo",
            });
        });

        //Act
        var result = await api.CreateClient().GetFromJsonAsync<JobDto[]>("Job");

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
    
    public JobControllerTests(PostgreSqlLifetime fixture) : base(fixture)
    {
    }
}