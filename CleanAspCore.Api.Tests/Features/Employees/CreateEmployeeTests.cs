using CleanAspCore.Api.Tests.Fakers;
using CleanAspCore.Features.Employees;

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

    [Test]
    public async Task CreateEmployee_InvalidRequest_ReturnsBadRequest()
    {
        //Arrange
        var createEmployeeRequest = new CreateEmployeeRequestFaker()
            .RuleFor(x => x.FirstName, string.Empty)
            .Generate();

        Sut.SeedData(context =>
        {
            context.Departments.Add(new DepartmentFaker().RuleFor(x => x.Id, createEmployeeRequest.DepartmentId).Generate());
            context.Jobs.Add(new JobFaker().RuleFor(x => x.Id, createEmployeeRequest.JobId).Generate());
        });

        //Act
        var response = await Sut.CreateClientFor<IEmployeeApiClient>().CreateEmployee(createEmployeeRequest);

        //Assert
        await response.AssertStatusCode(HttpStatusCode.BadRequest);
        Sut.AssertDatabase(context =>
        {
            context.Employees.Should().BeEmpty();
        });
    }
}
