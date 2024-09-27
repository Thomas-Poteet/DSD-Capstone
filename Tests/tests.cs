using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace DSD_Capstone.Tests;

[Parallelizable(ParallelScope.Self)] // This attribute makes the class run in parallel with other classes
[TestFixture] // This attribute makes the class a container for a group of related tests

// This class contains tests for the login page
public class LoginTests : PageTest
{
    // stores the number of users in the database
    private static readonly int intNumUsers = 4;

    // This method is called before each test
    [SetUp]
    public async Task GoToLogin()
    {
        await Page.GotoAsync("http://localhost:5208");
    }

    // check if the title is correct
    [Test]
    public async Task CheckLoginTitle()
    {
        await Expect(Page).ToHaveTitleAsync("Login - DSD_Capstone");
    }

    // check if datalist is filled with users
    [Test]
    public async Task CheckDatalist()
    {
        var options = await Page.QuerySelectorAllAsync("datalist option");
        Assert.That(options, Has.Count.EqualTo(intNumUsers));
    }

    // check error message when the username is empty
    [Test]
    public async Task EmptyUsername()
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page.GetByLabel("Please enter a username")).ToBeVisibleAsync();
    }

    // check error message when the user is not found
    [Test]
    public async Task UserNotFound()
    {
        await Page.GetByPlaceholder("Username").FillAsync("PParker");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page.GetByLabel("User not found")).ToBeVisibleAsync();
    }

    // check error message when the password is empty or invalid
    [Test]
    public async Task InvalidPassword()
    {
        await Page.GetByPlaceholder("Username").FillAsync("PParker1");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page.GetByLabel("Invalid admin password")).ToBeVisibleAsync();
    }

    [Test]
    public async Task SuccessfulLogin()
    {
        await Page.GetByPlaceholder("Username").FillAsync("PParker1");
        await Page.GetByPlaceholder("Password").FillAsync("111111");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Dashboard - DSD_Capstone");
    }
}