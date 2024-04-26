using System.Net;
using CleanAspCore.Domain;
using CleanAspCore.Domain.Jobs;
using CleanAspCore.Features.Import;

namespace CleanAspCore.Api.Tests.Features.Jobs;

public class JobControllerTests : TestBase
{
    [Test]
    public async Task SearchJobs_ReturnsExpectedJobs()
    {
        //Arrange
        var job = new JobFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Jobs.Add(job);
        });

        //Act
        var result = await Sut.CreateClient().GetFromJsonAsync<JobDto>($"jobs/{job.Id}");

        //Assert
        result.Should().BeEquivalentTo(job.ToDto());
    }

    [Test]
    public async Task AddJob_IsAdded()
    {
        //Arrange
        var job = new JobFaker().Generate();

        //Act
        var response = await Sut.CreateClient().PostAsJsonAsync("jobs", job.ToDto());
        await response.AssertStatusCode(HttpStatusCode.Created);
        var createdId = response.GetGuidFromLocationHeader();

        //Assert
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
