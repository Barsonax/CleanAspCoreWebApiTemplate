namespace CleanAspCore.Api.Tests.Features.Import;

public class ImportControllerTests
{
    private readonly TestWebApi _api;

    public ImportControllerTests(TestWebApi api)
    {
        _api = api;
    }
    
    [Fact]
    public async Task Import_SingleNewEmployee_IsImported()
    {
        //Act
        var result = await _api.CreateClient().PutAsync("Import", null);
        result.EnsureSuccessStatusCode();

        //Assert
        _api.AssertDatabase(context =>
        {
            context.Employees.Should().HaveCount(100);
        });
    }
}