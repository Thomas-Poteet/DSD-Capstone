using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace DSD_Capstone.Tests;

[Parallelizable(ParallelScope.Self)] // This attribute makes the class run in parallel with other classes
[TestFixture] // This attribute makes the class a container for a group of related tests
// This class inherits from PageTest which allows each test to get a fresh copy of a web page
public class Tests : PageTest
{
    [Test] // This attribute marks a method as a test method
    public async Task Test1()
    {
        await Page.GotoAsync("http://localhost:5208");

        await Expect(Page).ToHaveTitleAsync("Scenic Foods Direct Store Delivery System - DSD_Capstone");
    }
}