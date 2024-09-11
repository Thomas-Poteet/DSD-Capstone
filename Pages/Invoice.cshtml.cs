using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DSD_Capstone.Pages;

public class InvoiceModel : PageModel
{
    public List<string> Vendors { get; set; } = new List<string>();
    public List<string> InvoiceNumbers { get; set; } = new List<string>();
    public void OnGet()
    {
        // Lists needs to be replaced by db call. Most likely need
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

        InvoiceNumbers = new List<string>
        {
            "INV001234",
            "00056789",
            "A123456",
            "INV2024001",
            "B0023456",
            "00789012",
            "C789123",
            "00012345",
            "INV100987",
            "XYZ123456",
            "INV2023007",
            "D0012345",
            "00345678",
            "AB987654",
            "INV009876",
            "E0005678",
            "F0001234",
            "00456789",
            "INV2024005",
            "G0012345",
            "123456"
        };

    }
}
