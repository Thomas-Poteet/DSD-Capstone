using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DSD_Capstone.Pages;

public class LoginModel(MyDbContext context) : PageModel
{
    private readonly MyDbContext db = context;
    public List<string> Usernames { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        // Automatically logs user in if cookie is present
        if (User?.Identity != null && User.Identity.IsAuthenticated)
        {
            return RedirectToPage("/Dashboard");
        }

        // Query the database to fill the Usernames list
        Usernames = await db.Employees
            .Select(u => u.UserName)
            .ToListAsync();

        return Page();
    }
    public async Task<IActionResult> OnPostAsync(LoginRequest request)
    {
        // Check if the user exists in the database
        var user = await db.Employees
            .FirstOrDefaultAsync(u => u.UserName == request.Username);
        if (user == null)
        {
            return new JsonResult(new { success = false, message = "User not found" });
        }

        // Check if the password is correct
        if (user.AdminPassword != request.Password)
        {
            return new JsonResult(new { success = false, message = "Invalid admin password" });
        }


        // Cookie creation
        var claims = new List<Claim>() {
            new(ClaimTypes.Name, request.Username)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        return new JsonResult(new { success = true });
    }

    public class LoginRequest
    {
        public required string Username { get; set; }
        public required int Password { get; set; }
    }
}
