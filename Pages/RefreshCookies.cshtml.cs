using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DSD_Capstone.Pages;

public class RefreshCookiesModel : PageModel
{
    private readonly MyDbContext db; // Inject your DbContext

    public RefreshCookiesModel(MyDbContext dbContext)
    {
        db = dbContext;
    }
    public async Task<IActionResult> OnPostAsync()
    {
        if (User?.Identity != null && User.Identity.IsAuthenticated)
        {
            // Retrieve the SessionTimeout value from the database
            var sessionTimeoutSetting = await db.SiteSettings
                .FirstOrDefaultAsync(s => s.SettingKey == "SessionTimeout");
            if (sessionTimeoutSetting == null)
            {
                return new JsonResult(new { success = false, message = "Session timeout setting not found" });
            }

            // Parse the SessionTimeout value to an integer
            if (!double.TryParse(sessionTimeoutSetting.SettingValue, out double sessionTimeout))
            {
                return new JsonResult(new { success = false, message = "Invalid session timeout value" });
            }

            Console.WriteLine($"Session Timeout : {sessionTimeout}");

            var ClaimsPrincipal = HttpContext.User;

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(sessionTimeout)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, ClaimsPrincipal, authProperties);

            return new JsonResult(new { success = true });
        }
        return new JsonResult(new { success = true });
    }
}