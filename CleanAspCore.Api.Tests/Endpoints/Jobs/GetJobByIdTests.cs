using CleanAspCore.Api.TestUtils.Fakers;

namespace CleanAspCore.Api.Tests.Endpoints.Jobs;

internal sealed class GetJobByIdTests : TestBase
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
        await response.AssertJsonBodyIsEquivalentTo(new { Id = job.Id });
    }
}
