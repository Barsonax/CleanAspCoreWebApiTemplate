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
        var job = Fakers.CreateJobFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Jobs.Add(job);
        });

        //Act
        var result = await Sut.CreateClient().GetFromJsonAsync<JobDto[]>("Job");

        //Assert
        result.Should().BeEquivalentTo(new[]
        {
            job
        }, c => c.ComparingByMembers<Entity>().ExcludingMissingMembers());
    }
}
