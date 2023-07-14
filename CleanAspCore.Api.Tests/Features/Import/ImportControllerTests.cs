using CleanAspCore.Domain.Departments;
using CleanAspCore.Domain.Employees;
using CleanAspCore.Domain.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;

namespace CleanAspCore.Api.Tests.Features.Import;

public class ImportControllerTests
{
    private readonly TestWebApplicationFactory _api;

    public ImportControllerTests(TestWebApplicationFactory api)
    {
        _api = api;
    }
    
    [Fact]
    public async Task Import_SingleNewEmployee_IsImported()
    {
        _api.ConfigureServices(services =>
        {
            var fileProviderMock = new Mock<IFileProvider>()
                .SetupJsonFileMock("TestData/Employee.json", new[]
                {
                    new EmployeeDto()
                    {
                        Id = 1,
                        FirstName = "Foo",
                        LastName = "Bar",
                        Email = "email@foobar.com",
                        Gender = "Weird",
                        JobId = 2,
                        DepartmentId = 3
                    }
                })
                .SetupJsonFileMock("TestData/Job.json", Array.Empty<JobDto>())
                .SetupJsonFileMock("TestData/Department.json", Array.Empty<DepartmentDto>());

            services.Replace(new ServiceDescriptor(typeof(IFileProvider), fileProviderMock.Object));
        });

        _api.SeedData(context =>
        {
            context.Jobs.Add(new Job
            {
                Id = 2,
                Name = "Foo",
            });

            context.Departments.Add(new Department
            {
                Id = 3,
                Name = "Bar",
                City = "Foo"
            });
        });

        //Act
        var result = await _api.CreateClient().PutAsync("Import", null);
        result.EnsureSuccessStatusCode();

        //Assert
        _api.AssertDatabase(context =>
        {
            context.Employees.Should().BeEquivalentTo(new []
            {
                new Employee
                {
                    Id = 1,
                    FirstName = "Foo",
                    LastName = "Bar",
                    Email = "email@foobar.com",
                    Gender = "Weird",
                    JobId = 2,
                    DepartmentId = 3
                }
            });
        });
    }
}