using CleanAspCore.Features.Import;
using CleanAspCore.Features.Jobs;
using CleanAspCore.Features.Jobs.Endpoints;

namespace CleanAspCore.Api.Tests.Features.Jobs;

public class GetJobByIdTests : TestBase
{
    [Test]
    public async Task GetJobById_ReturnsExpectedJob()
    {
        //Arrange
        var job = new JobFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Jobs.Add(job);
        });

        //Act
        var response = await Sut.CreateClientFor<IJobApiClient>().GetJobById(job.Id);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.OK);
        var jobDto = await response.Content.ReadFromJsonAsync<JobDto>();
        jobDto.Should().BeEquivalentTo(new
        {
            Id = job.Id
        });
    }
}
