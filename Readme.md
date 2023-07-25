# Clean ASP.NET api template
This is a template repository showing how one can implement a clean api with ASP.NET.

Some 'features' in this template:
- Easy and fast to run Integration tests with the only dependency being docker with the use of [TestContainers](https://dotnet.testcontainers.org/) and [Respawn](https://github.com/jbogard/Respawn). 
- Vertical Slice architecture (grouping based on features instead of technical layers)
- CQS pattern with mediator

The application itself is currently just doing simple CRUD and as such alot of the patterns shown will be over engineered but the point is to experiment and show the patterns. Feel free to mix and match parts of it in your solutions. Don't forget to give a star while you are at it.

## Running tests

Only docker is required to run the tests. First run can take a bit longer as the docker image is downloaded.
Run the following command to run the tests:

```cmd
dotnet test
```
