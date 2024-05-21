using System.Security.Claims;
using CleanAspCore.Api.Tests.Fakers;
using CleanAspCore.Endpoints.Employees;

namespace CleanAspCore.Api.Tests.Features.Employees;

public class CreateEmployeeTests : TestBase
{
    [Test]
    public async Task CreateEmployee_IsAdded()
    {
        //Arrange
        var createEmployeeRequest = new CreateEmployeeRequestFaker().Generate();
        Sut.SeedData(context =>
        {
            context.Departments.Add(new DepartmentFaker().RuleFor(x => x.Id, createEmployeeRequest.DepartmentId).Generate());
            context.Jobs.Add(new JobFaker().RuleFor(x => x.Id, createEmployeeRequest.JobId).Generate());
        });

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteEmployeesClaim).CreateEmployee(createEmployeeRequest);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.Created);
        var createdId = response.GetGuidFromLocationHeader();
        Sut.AssertDatabase(context =>
        {
            context.Employees.Should().BeEquivalentTo(new[] { new { Id = createdId } });
        });
    }

    private static TestScenario<(FakerConfigurator<CreateEmployeeRequest>, string[])>[] _validationCases =
    [
        new("FirstName is null",
            (x => x.RuleFor(y => y.FirstName, (string?)null), ["FirstName"])),
        new("LastName is null",
            (x => x.RuleFor(y => y.LastName, (string?)null), ["LastName"])),
        new("Gender is null",
            (x => x.RuleFor(y => y.Gender, (string?)null), ["Gender"])),
        new("Email is null",
            (x => x.RuleFor(y => y.Email, (string?)null), ["Email"])),
        new("Invalid email",
            (x => x.RuleFor(y => y.Email, "this is not a valid email address"), ["Email"])),
        new("Job does not exist",
            (x => x.RuleFor(y => y.JobId, Guid.NewGuid()), ["JobId"])),
        new("Department does not exist",
            (x => x.RuleFor(y => y.DepartmentId, Guid.NewGuid()), ["DepartmentId"])),
    ];

    [TestCaseSource(nameof(_validationCases))]
    public async Task CreateEmployee_InvalidRequest_ReturnsBadRequest(TestScenario<(FakerConfigurator<CreateEmployeeRequest> configurator, string[] expectedErrors)> scenario)
    {
        //Arrange
        var departmentId = Guid.NewGuid();
        var jobId = Guid.NewGuid();

        var createEmployeeRequest = scenario.Input.configurator(new CreateEmployeeRequestFaker()
            .RuleFor(x => x.DepartmentId, departmentId)
            .RuleFor(x => x.JobId, jobId)).Generate();

        Sut.SeedData(context =>
        {
            context.Departments.Add(new DepartmentFaker().RuleFor(x => x.Id, departmentId).Generate());
            context.Jobs.Add(new JobFaker().RuleFor(x => x.Id, jobId).Generate());
        });

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteEmployeesClaim).CreateEmployee(createEmployeeRequest);

        //Assert
        await response.AssertBadRequest(scenario.Input.expectedErrors);
        Sut.AssertDatabase(context =>
        {
            context.Employees.Should().BeEmpty();
        });
    }
}
