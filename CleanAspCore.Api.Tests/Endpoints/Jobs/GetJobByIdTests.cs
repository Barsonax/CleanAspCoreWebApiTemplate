﻿using CleanAspCore.Api.Tests.Fakers;

namespace CleanAspCore.Api.Tests.Endpoints.Jobs;

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
        await response.AssertJsonBodyIsEquivalentTo(new { Id = job.Id });
    }
}
