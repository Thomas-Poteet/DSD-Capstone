using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DSD_Capstone.Pages;

public class LoginModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public LoginModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}