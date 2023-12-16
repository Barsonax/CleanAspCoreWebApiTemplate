namespace CleanAspCore.Api.Tests.Features.Import;

public class ImportControllerTests : TestBase
{
    [Test]
    public async Task Import_SingleNewEmployee_IsImported()
    {
        //Act
        var result = await Sut.CreateClient().PutAsync("Import", null);
        result.EnsureSuccessStatusCode();

        //Assert
        Sut.AssertDatabase(context =>
        {
            context.Employees.Should().HaveCount(100);
        });
    }
}
