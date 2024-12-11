# DSD-Capstone
## Setup for Testing
1. Download .NET 8.0 at https://dotnet.microsoft.com/en-us/download/dotnet/8.0
2. `dotnet build` to compile source code
3. Install Playwright Browsers:
    ### Windows
    - Download Powershell from the Microsoft Store and open it
    - `pwsh bin/Debug/net8.0/playwright.ps1 install` in DSD-Capstone directory
    ### Mac
    - ???
4. `dotnet run` to build and run the project
5. `dotnet test` in another terminal to run the tests

## Program.cs 
1. Prerequisites

    Microsoft.Data.SqlClient for connecting to SQL Server.
    Microsoft.EntityFrameworkCore for ORM mapping to the database.
    Swagger is included for API documentation and testing.

2. Database Configuration

    The database connection is set up using Entity Framework Core with SQL Server. The connection string is retrieved from the configuration.
    In appsettings.json, ensure you have your connection string for SQL Server:

3. Swagger Configuration

    Swagger is integrated into the project for API documentation and testing. The Swagger UI will be available in the development environment.
    You can access Swagger UI at /swagger when running the app in development mode.

4. Additional Configurations

    Static Files: This allows serving static files such as HTML, CSS, or JavaScript from the wwwroot directory.
    Routing & Authorization: The app uses Razor Pages for views and basic routing. Authentication is handled separately if needed.

    Running the Application

5. To run the application:

    Ensure you have SQL Server installed and configured.
    Update the appsettings.json file with the correct connection string.
    Run the application using dotnet run or from your IDE (e.g., Visual Studio).
    Access Swagger at https://localhost:<port>/swagger to view and test the API endpoints.