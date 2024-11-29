using CleanAspCore.Api.Endpoints.Employees;

namespace CleanAspCore.Api.Tests.Endpoints.Employees;

internal sealed class CreateEmployeeTests : TestBase
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
        var response = await Sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteRole).CreateEmployee(createEmployeeRequest);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.Created);
        var createdId = response.GetGuidFromLocationHeader();
        Sut.AssertDatabase(context =>
        {
            context.Employees.Should().BeEquivalentTo(new[] { new { Id = createdId } });
        });
    }

    private static readonly TestScenario<(FakerConfigurator<CreateEmployeeRequest>, string[])>[] _validationCases =
    [
        new((string)"FirstName is null",
            ((FakerConfigurator<CreateEmployeeRequest>, string[]))(x => x.RuleFor(y => y.FirstName, (string?)null), ["FirstName"])),
        new((string)"LastName is null",
            ((FakerConfigurator<CreateEmployeeRequest>, string[]))(x => x.RuleFor(y => y.LastName, (string?)null), ["LastName"])),
        new((string)"Gender is null",
            ((FakerConfigurator<CreateEmployeeRequest>, string[]))(x => x.RuleFor(y => y.Gender, (string?)null), ["Gender"])),
        new((string)"Email is null",
            ((FakerConfigurator<CreateEmployeeRequest>, string[]))(x => x.RuleFor(y => y.Email, (string?)null), ["Email"])),
        new((string)"Invalid email",
            ((FakerConfigurator<CreateEmployeeRequest>, string[]))(x => x.RuleFor(y => y.Email, "this is not a valid email address"), ["Email"])),
        new((string)"Job does not exist",
            ((FakerConfigurator<CreateEmployeeRequest>, string[]))(x => x.RuleFor(y => y.JobId, Guid.NewGuid()), ["JobId"])),
        new((string)"Department does not exist",
            ((FakerConfigurator<CreateEmployeeRequest>, string[]))(x => x.RuleFor(y => y.DepartmentId, Guid.NewGuid()), ["DepartmentId"])),
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
        var response = await Sut.CreateClientFor<IEmployeeApiClient>(ClaimConstants.WriteRole).CreateEmployee(createEmployeeRequest);

        //Assert
        await response.AssertBadRequest(scenario.Input.expectedErrors);
        Sut.AssertDatabase(context =>
        {
            context.Employees.Should().BeEmpty();
        });
    }
}
