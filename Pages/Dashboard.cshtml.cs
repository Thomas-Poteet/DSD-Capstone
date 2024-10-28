using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;


namespace DSD_Capstone.Pages;

[Authorize]
public class DashboardModel(MyDbContext context) : PageModel
{
    private readonly MyDbContext db = context;

    public List<string> Vendors { get; set; } = new List<string>();
    public async Task OnGetAsync()
    {
        // Query the database to fill the Vendors list
        Vendors = await db.Vendors
            .Select(v => v.name)
            .ToListAsync();

        // Get the employee name from the cookie
        var username = User.FindFirstValue(ClaimTypes.Name);
        var employee = await db.Employees
            .FirstOrDefaultAsync(e => e.UserName == username);
        if (employee == null)
        {
            return;
        }  
        var emp_name = employee.FirstName + " " + employee.LastName;
    }
    
    
    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return new JsonResult(new { success = true });
    }
}

