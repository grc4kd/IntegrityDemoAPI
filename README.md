# README

This demonstration project has a Web API using .NET 8 and ASP.NET. The controllers and their runtime environment are run as a program spawned by a .NET Run Command process. There is an environment variable available to set the program's currency code, but the only implemented currency is US Dollars.

## Prerequisites
Make sure that the dotnet 8 SDK and EF core tools are installed.
```
dotnet tool install --global dotnet-ef
```

On Linux, you will need to use a third-party dotnet tool or manually set up the developer certificate.
https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-8.0&tabs=visual-studio%2Clinux-ubuntu#trust-https-certificate-on-linux

### Trust HTTPS certificate on Linux with linux-dev-certs
```
dotnet tool update -g linux-dev-certs
dotnet linux-dev-certs install
```

## Build
To build and test the program:

```
dotnet test
```

The tests will automatically set the environment variable `DefaultCurrencyCode` to string value "`USD`" using a xUnit test class fixture.

## Run
To build and run the program:

```
cd WebApi/
dotnet ef database update
```

With the WebApi folder as the working directory, `dotnet ef database update` will create the required Sqlite database file for this demo.

```
dotnet run
```

A .NET Host should create a Kestrel cross-platform web server and output the host ports on the command line. The app is configured to always use HTTPS and will fail if a valid certificate is not installed. A developer certificate may be used on local machines.

```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7122
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
```

In a web browser, visit http://localhost:7122/swagger. The page should display the Swagger API Documentation created via Swashbuckle middleware.

Simple test data is seeded when running the .NET run command for the first time.

Environment variables for `dotnet run` are defined in `launchSettings.json`.

The first `dotnet run` should generate a self-signed certificate to enable HTTPS use in development on Windows machines. On Linux machines, the open-source tool `dotnet linux-dev-certs` is used and configures a trusted developer certificate for use with Firefox.
