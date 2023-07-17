# Clean ASP.NET api template
This is a template repository showing how one can implement a clean api with ASP.NET.

Some 'features' in this template:
- Easy and fast to run Integration tests with the only dependency being docker. 
- Vertical Slice architecture (grouping based on features instead of technical layers)
- CQS pattern with mediator

The application itself is currently just doing simple CRUD and as such alot of the patterns shown will be over engineered but the point is to experiment and show the patterns.

## Running tests

Only docker is required to run the tests. First run can take a bit longer as the docker image is downloaded.
Run the following command to run the tests:

```cmd
dotnet test
```


## Setting up https for when running in a docker container

Run these commands to generate a https certificate for development:

```cmd
 dotnet dev-certs https -ep "$env:USERPROFILE\.aspnet\https\aspnetapp.pfx"  -p "password"
 dotnet dev-certs https --trust
```
