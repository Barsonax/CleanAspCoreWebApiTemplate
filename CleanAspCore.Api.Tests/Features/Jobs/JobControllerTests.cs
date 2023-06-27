using System.Net.Http.Json;
using CleanAspCore.Api.Tests.Helpers;
using CleanAspCore.Domain.Job;
using FluentAssertions;

namespace CleanAspCore.Api.Tests.Features.Jobs;

public class JobControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task SearchJobs_ReturnsExpectedJobs()
    {
        //Arrange
        Context.Jobs.Add(new Job()
        {
            Id = 0,
            Name = "Foo",
        });

        await Context.SaveChangesAsync();
        
        //Act
        var result = await Client.GetFromJsonAsync<JobDto[]>("Job");

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