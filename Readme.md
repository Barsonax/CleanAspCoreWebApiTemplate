## Setting up https

Run these commands to generate a https certificate for development:

```cmd
 dotnet dev-certs https -ep "$env:USERPROFILE\.aspnet\https\aspnetapp.pfx"  -p "password"
 dotnet dev-certs https --trust
```