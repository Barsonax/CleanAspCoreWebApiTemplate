using CleanAspCore.Api.Tests.Fakers;
using CleanAspCore.Features.Employees.Endpoints;

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
        var response = await Sut.CreateClientFor<IEmployeeApiClient>().CreateEmployee(createEmployeeRequest);

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
    ];

    [TestCaseSource(nameof(_validationCases))]
    public async Task CreateEmployee_InvalidRequest_ReturnsBadRequest(TestScenario<(FakerConfigurator<CreateEmployeeRequest> configurator, string[] expectedErrors)> scenario)
    {
        //Arrange
        var createEmployeeRequest = scenario.Input.configurator(new CreateEmployeeRequestFaker()).Generate();

        Sut.SeedData(context =>
        {
            context.Departments.Add(new DepartmentFaker().RuleFor(x => x.Id, createEmployeeRequest.DepartmentId).Generate());
            context.Jobs.Add(new JobFaker().RuleFor(x => x.Id, createEmployeeRequest.JobId).Generate());
        });

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>().CreateEmployee(createEmployeeRequest);

        //Assert
        await response.AssertBadRequest(scenario.Input.expectedErrors);
        Sut.AssertDatabase(context =>
        {
            context.Employees.Should().BeEmpty();
        });
    }
}
