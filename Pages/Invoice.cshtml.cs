using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DSD_Capstone.Pages;

[Authorize]
public class InvoiceModel(MyDbContext context) : PageModel
{
    private readonly MyDbContext db = context;

    public List<string> Vendors { get; set; } = [];
    public async Task OnGetAsync()
    {
        // Query the database to fill the Vendors list
        Vendors = await db.Vendors
            .Select(v => v.name)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostAsync(InvoiceRequest request)
    {
        // Get vendor number
        var vendor = await db.Vendors
            .FirstOrDefaultAsync(v => v.name == request.VendorName);
        if (vendor == null)
        {
            return new JsonResult(new { success = false, duplicate = false });
        }
        var vendor_no = vendor.vendor_no;

        // Check if invoice number already exists for the vendor
        var invoice = await db.Invoices
            .FirstOrDefaultAsync(i => i.InvoiceID == request.InvoiceID && i.vendor_no == vendor_no);
        if (invoice != null && !request.Update)
        {
            return new JsonResult(new { success = false, duplicate = true, });
        }

        // Get the employee number from the cookie
        var username = User.FindFirstValue(ClaimTypes.Name);
        var employee = await db.Employees
            .FirstOrDefaultAsync(e => e.UserName == username);
        if (employee == null)
        {
            return RedirectToPage("/Login");
        }  
        var emp_no = employee.emp_no;

        if (request.Update)
        {
            // Update existing invoice
            invoice!.emp_no = emp_no;
            invoice.vendor_no = vendor_no;
            invoice.Date = DateOnly.Parse(request.Date);
            invoice.vendor_total = request.VendorTotal;

            // Update the products in the invoice or remove them if they are not in the request
            var matchingProducts = await db.InvoiceProducts
                .Where(ip => ip.InvoiceID == request.InvoiceID && ip.vendor_no == vendor_no)
                .ToListAsync();
            foreach (var product in matchingProducts)
            {
                if (request.UPCs.Contains(product.upc))
                {
                    var index = Array.IndexOf(request.UPCs, product.upc);
                    product.count = request.Counts[index];

                    request.UPCs[index] = "";

                    db.InvoiceProducts.Update(product);
                }
                else
                {
                    db.InvoiceProducts.Remove(product);
                }
            }

            // Add new products to the invoice
            for (int i = 0; i < request.UPCs.Length; i++)
            {
                var upc = request.UPCs[i];
                if (upc == "")
                {
                    continue;
                }
                var count = request.Counts[i];


                var newInvoiceProduct = new InvoiceProduct
                {
                    InvoiceID = request.InvoiceID,
                    upc = upc,
                    count = count,
                    vendor_no = vendor_no,
                    Invoice = invoice
                };
                db.InvoiceProducts.Add(newInvoiceProduct);
            }

        } else {
            // Create new invoice
            var newInvoice = new Invoice
            {
                InvoiceID = request.InvoiceID,
                emp_no = emp_no,
                vendor_no = vendor_no,
                Date = DateOnly.Parse(request.Date),
                vendor_total = request.VendorTotal,
                InvoiceProducts = []
            };

            // Add the products to the invoice
            for (int i = 0; i < request.UPCs.Length; i++)
            {
                var upc = request.UPCs[i];
                var count = request.Counts[i];

                var newInvoiceProduct = new InvoiceProduct
                {
                    InvoiceID = request.InvoiceID,
                    upc = upc,
                    count = count,
                    vendor_no = vendor_no,
                    Invoice = newInvoice
                };
                newInvoice.InvoiceProducts.Add(newInvoiceProduct);
            }

            // Add the new invoice to the database
            db.Invoices.Add(newInvoice);
        }

        await db.SaveChangesAsync();
        return new JsonResult(new { success = true });
    }

    public class InvoiceRequest
    {
        public required string InvoiceID { get; set; }
        public required string VendorName { get; set; }
        public required string Date { get; set; }
        public required decimal VendorTotal { get; set; }
        public required string[] UPCs { get; set; }
        public required int[] Counts { get; set; }
        public required bool Update { get; set; }
    }
}
