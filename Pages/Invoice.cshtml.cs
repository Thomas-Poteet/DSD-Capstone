using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DSD_Capstone.Pages;

public class InvoiceModel : PageModel
{
    public List<string> Vendors { get; set; } = new List<string>();
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
}
