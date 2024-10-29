using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DSD_Capstone.Pages;

public class CheckCookiesModel : PageModel
{
    public IActionResult OnGet()
    {
        if (User?.Identity != null && User.Identity.IsAuthenticated)
        {
            return new JsonResult(new { success = true });
        }
        return new JsonResult(new { success = false });
    }
}