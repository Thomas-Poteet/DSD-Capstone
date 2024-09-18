using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DSD_Capstone.Pages;

public class InvoiceModel : PageModel
{
    private readonly MyDbContext vendors;
    public InvoiceModel(MyDbContext context)
        {
            vendors = context;
        }
    public List<string> Vendors { get; set; } = new List<string>();
    public async Task OnGetAsync()
    {
        // Query the database to fill the Vendors list
        Vendors = await vendors.Vendors
            .Select(v => v.name)
            .ToListAsync();
    }
    /*
    public void OnGet()
    {
        // List needs to be replaced by db call. Most likely need
        // .ToList() at the end of the call.
        Vendors = new List<string>
        {
            "Sysco",
            "US Foods",
            "C&S Wholesale Grocers",
            "UNFI",
            "KeHE Distributors",
            "Gordon Food Service",
            "SpartanNash",
            "PepsiCo",
            "Coca-Cola Bottling Co.",
            "Keurig Dr Pepper",
            "Dairy Farmers of America",
            "Flowers Foods",
            "Tyson Foods",
            "JBS USA",
            "FreshPoint"
        };
    }
    */
}
