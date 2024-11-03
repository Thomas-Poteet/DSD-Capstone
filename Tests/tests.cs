using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace DSD_Capstone.Tests;

[Parallelizable(ParallelScope.Self)] // This attribute makes the class run in parallel with other classes
[TestFixture] // This attribute makes the class a container for a group of related tests

// This class contains tests for the login page
public class LoginTests : PageTest
{
    // This method is called before each test and navigates to the login page
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

    // check if dashboard redirects to login if the user is not logged in
    [Test]
    public async Task RedirectDashboard()
    {
        await Page.GotoAsync("http://localhost:5208/Dashboard");
        await Expect(Page).ToHaveTitleAsync("Login - DSD_Capstone");
    }

    // check if invoice page redirects to login if the user is not logged in
    [Test]
    public async Task RedirectInvoice()
    {
        await Page.GotoAsync("http://localhost:5208/Invoice");
        await Expect(Page).ToHaveTitleAsync("Login - DSD_Capstone");
    }

    // check if datalist is filled with users
    [Test]
    public async Task CheckDatalist()
    {
        var options = await Page.QuerySelectorAllAsync("#dlUsernames option");
        Assert.That(options, Has.Count.Not.EqualTo(0));
    }

    // check error message when the username is empty
    [Test]
    public async Task EmptyUsername()
    {
        await Page.GetByPlaceholder("Password").FillAsync("111111");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page.GetByLabel("Please enter a username and password")).ToBeVisibleAsync();
    }

    // check error message when the user is not found
    [Test]
    public async Task UserNotFound()
    {
        await Page.GetByPlaceholder("Username").FillAsync("PParker");
        await Page.GetByPlaceholder("Password").FillAsync("111111");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page.GetByLabel("User not found")).ToBeVisibleAsync();
    }

    // check error message when the password is empty or invalid
    [Test]
    public async Task InvalidPassword()
    {
        await Page.GetByPlaceholder("Username").FillAsync("PParker1");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page.GetByLabel("Please enter a username and password")).ToBeVisibleAsync();
    }

    // check if the login is successful with valid credentials
    [Test]
    public async Task SuccessfulLogin()
    {
        await Page.GetByPlaceholder("Username").FillAsync("PParker1");
        await Page.GetByPlaceholder("Password").FillAsync("111111");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Dashboard - DSD_Capstone");
    }
}

public class DashboardTests : PageTest
{
    // This method is called before each test and navigates to the dashboard
    [SetUp]
    public async Task GoToDashboard()
    {
        await Page.GotoAsync("http://localhost:5208");
        await Page.GetByPlaceholder("Username").FillAsync("PParker1");
        await Page.GetByPlaceholder("Password").FillAsync("111111");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
    }

    // check if login redirects to dashboard if the user is logged in
    // [Test]
    // public async Task AutoLogin()
    // {
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Logout" }).ClickAsync();
    //     await Page.GetByPlaceholder("Username").FillAsync("PParker1");
    //     await Page.GetByPlaceholder("Password").FillAsync("111111");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
    //     await Page.ReloadAsync();
    //     await Expect(Page).ToHaveTitleAsync("Dashboard - DSD_Capstone");
    // }

    // check if Create Invoice button works
    [Test]
    public async Task CreateInvoiceButton()
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = "Create Invoice" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Create Invoice - DSD_Capstone");
        await Expect(Page.GetByRole(AriaRole.Heading)).ToContainTextAsync("Save Invoice");
    }

    // check if Logout button works
    [Test]
    public async Task LogoutButton()
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = "Logout" }).ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Login - DSD_Capstone");
        await Page.GotoAsync("http://localhost:5208/Dashboard");
        await Expect(Page).ToHaveTitleAsync("Login - DSD_Capstone");
    }
}

public class InvoiceTests : PageTest
{
    // This method is called before each test and navigates to the invoice page
    [SetUp]
    public async Task GoToInvoice()
    {
        await Page.GotoAsync("http://localhost:5208");
        await Page.GetByPlaceholder("Username").FillAsync("PParker1");
        await Page.GetByPlaceholder("Password").FillAsync("111111");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Page.GetByRole(AriaRole.Button, new() { Name = "Create Invoice" }).ClickAsync();
    }

    // check if the back button works
    [Test]
    public async Task BackButton()
    {
        await Page.Locator("#btnBack").ClickAsync();
        await Expect(Page).ToHaveTitleAsync("Dashboard - DSD_Capstone");
    }

    // check if vendor datalist is filled
    [Test]
    public async Task CheckVendorDatalist()
    {
        var options = await Page.QuerySelectorAllAsync("#dlVendors option");
        Assert.That(options, Has.Count.Not.EqualTo(0));
    }

    // check if the date is filled with todays date
    [Test]
    public async Task CheckDate()
    {
        await Expect(Page.GetByLabel("Date")).ToHaveValueAsync(DateTime.Now.ToString("yyyy-MM-dd"));
    }

    // check that error message is displayed when UPC is empty
    [Test]
    public async Task EmptyUPC()
    {
        await Page.GetByRole(AriaRole.Button, new() { Name = "Add Product" }).ClickAsync();
        await Expect(Page.GetByText("Please enter a UPC")).ToBeVisibleAsync();
    }

    // check that error message is displayed when UPC is invalid
    // [Test]
    // public async Task InvalidUPC()
    // {
    //     await Page.GetByLabel("Vendor", new() { Exact = true }).FillAsync("M&M Distributions");
    //     await Page.GetByPlaceholder("UPC").FillAsync("123456789012");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Add Product" }).ClickAsync();
    //     await Expect(Page.GetByText("Product not found or an error occurred")).ToBeVisibleAsync();
    // }

    // // check that valid UPC is accepted
    // [Test]
    // public async Task ValidUPC()
    // {
    //     await Page.GetByLabel("Vendor", new() { Exact = true }).FillAsync("M&M Distributions");
    //     await Page.GetByPlaceholder("UPC").FillAsync("013000002523");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Add Product" }).ClickAsync();
    //     await Expect(Page.GetByRole(AriaRole.Heading, new() { Name = "HEINZ Kethchup" })).ToBeVisibleAsync();

    //     // empty count
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
    //     await Expect(Page.GetByText("You need to enter a number!")).ToBeVisibleAsync();

    //     // negative count
    //     await Page.GetByLabel("Enter Item Count").FillAsync("-1");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
    //     await Expect(Page.GetByText("Count cannot be negative!")).ToBeVisibleAsync();

    //     // decimal count
    //     await Page.GetByLabel("Enter Item Count").FillAsync("1.1");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
    //     await Expect(Page.GetByText("Count must be a whole number!")).ToBeVisibleAsync();

    //     // zero count
    //     await Page.GetByLabel("Enter Item Count").FillAsync("0");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
    //     await Expect(Page.GetByText("Count must be greater than 0!")).ToBeVisibleAsync();

    //     // >999 count
    //     await Page.GetByLabel("Enter Item Count").FillAsync("1000");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
    //     await Expect(Page.GetByText("Count must be less than 1000!")).ToBeVisibleAsync();

    //     // valid count
    //     await Page.GetByLabel("Enter Item Count").FillAsync("10");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
    //     await Expect(Page.GetByText("Retail: $14.99 Total Retail: $149.90")).Not.ToBeVisibleAsync();
    //     await Page.Locator("#divItemsCard div").Filter(new() { HasText = "HEINZ Kethchup - 10" }).Nth(1).ClickAsync();
    //     await Expect(Page.GetByText("Retail: $14.99 Total Retail: $149.90")).ToBeVisibleAsync();
    // }

    // // check that error message is displayed when a duplicate UPC is entered
    // [Test]
    // public async Task DuplicateUPC()
    // {
    //     await Page.GetByLabel("Vendor", new() { Exact = true }).FillAsync("M&M Distributions");
    //     await Page.GetByPlaceholder("UPC").FillAsync("013000002523");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Add Product" }).ClickAsync();
    //     await Page.GetByLabel("Enter Item Count").FillAsync("10");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
    //     await Page.GetByPlaceholder("UPC").FillAsync("013000002523");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Add Product" }).ClickAsync();
    //     await Expect(Page.GetByText("Cannot add duplicate item to invoice")).ToBeVisibleAsync();
    // }

    // // check that the totals are correct
    // [Test]
    // public async Task CheckTotals()
    // {
    //     await Page.GetByLabel("Vendor", new() { Exact = true }).FillAsync("M&M Distributions");
    //     await Page.GetByPlaceholder("UPC").FillAsync("013000002523");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Add Product" }).ClickAsync();
    //     await Page.GetByLabel("Enter Item Count").FillAsync("10");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
    //     await Page.GetByPlaceholder("UPC").FillAsync("049000050233");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Add Product" }).ClickAsync();
    //     await Page.GetByLabel("Enter Item Count").FillAsync("5");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
    //     await Expect(Page.Locator("form")).ToContainTextAsync("Units: 15");
    //     // TODO: needs correct cost and margin once we start using the API
    //     // await Expect(Page.Locator("form")).ToContainTextAsync("Cost: $");
    //     // await Expect(Page.Locator("form")).ToContainTextAsync("Gross Margin: %");
    //     await Expect(Page.Locator("form")).ToContainTextAsync("Retail: $174.85");
    // }

    // // check that error message is displayed when the vendor is empty
    // [Test]
    // public async Task EmptyVendor()
    // {
    //     await Page.GetByLabel("Invoice Number").FillAsync("123456789");
    //     await Page.GetByLabel("Vendor Total").FillAsync("0.00");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
    //     await Expect(Page.GetByText("Please fill out all fields")).ToBeVisibleAsync();
    // }

    // // check that error message is displayed when the invoice number is empty
    // [Test]
    // public async Task EmptyInvoiceNumber()
    // {
    //     await Page.GetByLabel("Vendor", new() { Exact = true }).FillAsync("M&M Distributions");
    //     await Page.GetByLabel("Vendor Total").FillAsync("0.00");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
    //     await Expect(Page.GetByText("Please fill out all fields")).ToBeVisibleAsync();
    // }

    // // check that error message is displayed when the vendor total is empty
    // [Test]
    // public async Task EmptyVendorTotal()
    // {
    //     await Page.GetByLabel("Vendor", new() { Exact = true }).FillAsync("M&M Distributions");
    //     await Page.GetByLabel("Invoice Number").FillAsync("123456789");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
    //     await Expect(Page.GetByText("Please fill out all fields")).ToBeVisibleAsync();
    // }

    // // check that error message is displayed when no products are added
    // [Test]
    // public async Task NoProducts()
    // {
    //     await Page.GetByLabel("Vendor", new() { Exact = true }).FillAsync("M&M Distributions");
    //     await Page.GetByLabel("Invoice Number").FillAsync("123456789");
    //     await Page.GetByLabel("Vendor Total").FillAsync("0.00");
    //     await Page.GetByRole(AriaRole.Button, new() { Name = "Save" }).ClickAsync();
    //     await Expect(Page.GetByText("Please add at least one item")).ToBeVisibleAsync();
    // }
}