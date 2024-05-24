# Clean ASP.NET minimal api template

This is a template repository showing how one can implement a clean api with ASP.NET using minimal apis.

Some 'features' in this template:

- [Vertical Slice architecture](https://www.jimmybogard.com/vertical-slice-architecture/) (grouping based on features instead of technical layers)
- An easy to use and fast to run integration tests setup that runs in seconds with the only dependency being docker with the use
  of [TestExamplesDotnet](https://github.com/Barsonax/TestExamplesDotnet)
- Authentication and authorization using jwt tokens. This is also used by the tests which means you can check if your authentication and authorization is working properly inside the tests.
- Ready for [OpenTelemetry](https://opentelemetry.io/)
- Launchprofile for [dotnet watch](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-watch)

## Running tests

Only docker is required to run the tests. First run can take a bit longer as the docker image is downloaded.
Run the following command to run the tests:

```cmd
dotnet test
```

## Running the app

1. First generate a jwt that you can use for local testing:

```cmd
dotnet user-jwts create --role "reademployees" --role "writeemployees"
```

NOTE: The jobs and department endpoints only require authentication but the employee endpoints require that you have the correct claims in the jwt token.

2. Run the database using the provided docker-compose.yaml. Optionally also run the aspire dashboard in the compose file to easily see OpenTelemetry output.
3. Run the app. You can explore the endpoints using swagger at `https://localhost:7162/swagger`.
