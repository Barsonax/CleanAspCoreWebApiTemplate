using System.Net.Http.Json;
using CleanAspCore.Api.Tests.Helpers;
using CleanAspCore.Domain;

namespace CleanAspCore.Api.Tests;

public class IntegrationTest2 : IntegrationTestBase
{
    [Fact]
    public async Task ExecuteCommand()
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
        var job = result.Content.ReadFromJsonAsync<JobDto>();

        //Assert


    }
    
    public IntegrationTest2(PostgreSqlLifetime fixture) : base(fixture)
    {
    }
}