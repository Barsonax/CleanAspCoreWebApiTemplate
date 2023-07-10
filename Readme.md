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