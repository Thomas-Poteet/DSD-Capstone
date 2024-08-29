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