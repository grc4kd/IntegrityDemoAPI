# README

This demonstration project has a Web API using .NET 8 and ASP.NET. The controllers and their runtime environment are run as a program spawned by a .NET Run Command process. There is an environment variable available to set the program's currency code, but the only implemented currency is US Dollars.

## Build
To build and test the program:

- dotnet test

## Run
To build and run the program:

- cd WebApi/
- dotnet ef database update

With the WebApi folder as the working directory, `dotnet ef database update` will create the required Sqlite database file for this demo.

- dotnet run --profile https  

A .NET Host should create a Kestrel cross-platform web server and output the host ports on the command line. Either http or https endpoint should be redirected to use HTTPS in middleware.

```
Building...
Hosting environment: Development
Content root path: /home/user/Downloads/IntegrityDemoAPI-release/WebApi
Now listening on: http://localhost:5135
Application started. Press Ctrl+C to shut down.

```

In a web browser, visit http://localhost:5135/swagger. The page should display the Swagger API Documentation created via Swashbuckle middleware.

Requirement:
There is no seed data with this project yet, though the tests have seed data that is not persisted anywhere. Records have to be added to the database file with a sqlcmd tool or database application before running.

