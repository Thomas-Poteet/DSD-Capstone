using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DSD_Capstone.Pages;

public class LoginModel : PageModel
{
    private readonly MyDbContext db;

    public LoginModel(MyDbContext context)
    {
        db = context;
    }
    public List<string> Usernames { get; set; } = new List<string>();
    public async Task OnGetAsync()
    {
        // Query the database to fill the Usernames list
        Usernames = await db.Employees
            .Select(u => u.UserName)
            .ToListAsync();
    }
}
