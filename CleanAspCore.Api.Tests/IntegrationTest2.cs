using System.Net.Http.Json;
using CleanAspCore.Api.Tests.Helpers;
using CleanAspCore.Domain;
using FluentAssertions;

namespace CleanAspCore.Api.Tests;

public class IntegrationTest2 : IntegrationTestBase
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
        var result = await Client.GetAsync("Job");

        result.EnsureSuccessStatusCode();
        var jobs = await result.Content.ReadFromJsonAsync<JobDto[]>();

        //Assert
        jobs.Should().BeEquivalentTo(new[]
        {
            new JobDto()
            {
                Id = 0,
                Name = "Foo",
            }
        });
    }
    
    public IntegrationTest2(PostgreSqlLifetime fixture) : base(fixture)
    {
    }
}