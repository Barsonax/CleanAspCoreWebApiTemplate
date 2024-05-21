# Clean ASP.NET api template
This is a template repository showing how one can implement a clean api with ASP.NET using minimal apis.

Some 'features' in this template:
- Vertical Slice architecture (grouping based on features instead of technical layers)
- An easy to use and fast to run integration tests setup with the only dependency being docker with the use of [TestExamplesDotnet](https://github.com/Barsonax/TestExamplesDotnet)

## Running tests

Only docker is required to run the tests. First run can take a bit longer as the docker image is downloaded.
Run the following command to run the tests:

```cmd
dotnet test
```

## Running the app
First generate a jwt that you can use for local testing:
```cmd
dotnet user-jwts create --claim "reademployees=" --claim "writeemployees="
```

Then run the database using the provided docker-compose.yaml then run the app.
