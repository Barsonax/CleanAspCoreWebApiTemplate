using CleanAspCore.Domain.Departments;
using CleanAspCore.Domain.Employees;
using CleanAspCore.Domain.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;

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
        var employee = Fakers.CreateEmployeeFaker().Generate();
        _api.ConfigureServices(services =>
        {
            var fileProviderMock = new Mock<IFileProvider>()
                .SetupJsonFileMock("TestData/Employee.json", new[]
                {
                    employee.ToDto()
                })
                .SetupJsonFileMock("TestData/Job.json", Array.Empty<JobDto>())
                .SetupJsonFileMock("TestData/Department.json", Array.Empty<DepartmentDto>());

            services.Replace(new ServiceDescriptor(typeof(IFileProvider), fileProviderMock.Object));
        });

        _api.SeedData(context =>
        {
            context.Jobs.Add(employee.Job!);
            context.Departments.Add(employee.Department!);
        });

        //Act
        var result = await _api.CreateClient().PutAsync("Import", null);
        result.EnsureSuccessStatusCode();

        //Assert
        _api.AssertDatabase(context =>
        {
            context.Employees.Should().BeEquivalentTo(new []
            {
                employee
            });
        });
    }
}