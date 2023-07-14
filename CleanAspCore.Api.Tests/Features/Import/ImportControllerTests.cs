using CleanAspCore.Domain.Departments;
using CleanAspCore.Domain.Employees;
using CleanAspCore.Domain.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Xunit.Abstractions;

namespace CleanAspCore.Api.Tests.Features.Import;

public class ImportControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task Import_SingleNewEmployee_IsImported()
    {
        await using var api = CreateApi().ConfigureServices(services =>
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

        api.SeedData(context =>
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
        var result = await api.CreateClient().PutAsync("Import", null);
        result.EnsureSuccessStatusCode();

        //Assert
        api.AssertDatabase(context =>
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

    public ImportControllerTests(PostgreSqlLifetime fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }
}