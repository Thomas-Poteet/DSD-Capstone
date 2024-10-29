using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DSD_Capstone.Pages;

public class RefreshCookiesModel : PageModel
{
    public async Task<IActionResult> OnPostAsync()
    {
        if (User?.Identity != null && User.Identity.IsAuthenticated)
        {
            var ClaimsPrincipal = HttpContext.User;

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, ClaimsPrincipal, authProperties);

            return new JsonResult(new { success = true });
        }
        return new JsonResult(new { success = true });
    }
}