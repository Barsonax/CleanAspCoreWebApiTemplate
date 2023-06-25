using System.Text.Json;
using CleanAspCore.Domain;

namespace CleanAspCore.Api.Tests;

public class ImportControllerTests
{
    [Fact]
    public async Task foo()
    {
        var foo = 
            """
[
        {
            "id": 1,
            "firstname": "Neal",
            "lastname": "Collopy",
            "email": "ncollopy0@slate.com",
            "gender": "Male",
            "Department": 1,
            "Job": 2
        }]
""";
        var result = JsonSerializer.Deserialize<EmployeeDto[]>(foo);
    }
}