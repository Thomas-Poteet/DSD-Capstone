using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
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

    public async Task<IActionResult> OnGetInvoiceAsync(string vendorName, string invoiceID) {
        // Get vendor number
        var vendor = await db.Vendors
            .FirstOrDefaultAsync(v => v.name == vendorName);
        if (vendor == null)
        {
            return new JsonResult(new { success = false, duplicate = false });
        }
        var vendor_no = vendor.vendor_no;

        // Check if invoice number already exists for the vendor
        var invoice = await db.Invoices
            .FirstOrDefaultAsync(i => i.InvoiceID == invoiceID && i.vendor_no == vendor_no);
        if (invoice == null)
        {
            return new JsonResult(new { success = true, duplicate = false });
        }
        return new JsonResult(new { success = true, duplicate = true });
    }

    public async Task<IActionResult> OnGetFillInvoiceAsync(string vendorName, string invoiceID) {
        // Get vendor number
        var vendor = await db.Vendors
            .FirstOrDefaultAsync(v => v.name == vendorName);
        if (vendor == null)
        {
            return new JsonResult(new { success = false, duplicate = false });
        }
        var vendor_no = vendor.vendor_no;

        // Check if invoice number already exists for the vendor
        var invoice = await db.Invoices
            .FirstOrDefaultAsync(i => i.InvoiceID == invoiceID && i.vendor_no == vendor_no);
        if (invoice == null)
        {
            return new JsonResult(new { success = true, duplicate = false });
        }

        var Products = await db.InvoiceProducts
                .Where(ip => ip.InvoiceID == invoiceID && ip.vendor_no == vendor_no)
                .ToListAsync();
        var arrUPCs = Products.Select(p => p.upc).ToArray();
        var arrCounts = Products.Select(p => p.count).ToArray();
        var arrAllowances = Products.Select(p => p.allowances).ToArray();
        var arrRetailCost = Products.Select(p => p.retail_cost).ToArray();
        var arrVendorCost = Products.Select(p => p.vendor_cost).ToArray();
        var arrNetCost = Products.Select(p => p.net_cost).ToArray();

        return new JsonResult(new {
            success = true,
            duplicate = true,
            date = invoice.Date,
            vendorTotal = invoice.vendor_total,
            retailTotal = invoice.retail_total,
            grossTotal = invoice.gross,
            countTotal = invoice.count_total,
            arrUPCs,
            arrCounts,
            arrAllowances,
            arrRetailCost,
            arrVendorCost,
            arrNetCost
        });
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
            invoice.retail_total = request.RetailTotal;
            invoice.gross = request.Gross;
            invoice.count_total = request.CountTotal;

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
                var retail_cost = request.RetailCosts[i];
                var allowances = request.Allowances[i];
                var vendor_cost = request.VendorCosts[i];
                var net_cost = request.NetCosts[i];


                var newInvoiceProduct = new InvoiceProduct
                {
                    InvoiceID = request.InvoiceID,
                    upc = upc,
                    count = count,
                    vendor_no = vendor_no,
                    retail_cost = retail_cost,
                    allowances = allowances,
                    vendor_cost = vendor_cost,
                    net_cost = net_cost,
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
                retail_total = request.RetailTotal,
                gross = request.Gross,
                count_total = request.CountTotal,
                InvoiceProducts = []
            };

            // Add the products to the invoice
            for (int i = 0; i < request.UPCs.Length; i++)
            {
                var upc = request.UPCs[i];
                var count = request.Counts[i];
                var retail_cost = request.RetailCosts[i];
                var allowances = request.Allowances[i];
                var vendor_cost = request.VendorCosts[i];
                var net_cost = request.NetCosts[i];

                var newInvoiceProduct = new InvoiceProduct
                {
                    InvoiceID = request.InvoiceID,
                    upc = upc,
                    count = count,
                    vendor_no = vendor_no,
                    retail_cost = retail_cost,
                    allowances = allowances,
                    vendor_cost = vendor_cost,
                    net_cost = net_cost,
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
        public required decimal RetailTotal { get; set; }
        public required double Gross { get; set; }
        public required int CountTotal { get; set; }
        public required string[] UPCs { get; set; }
        public required int[] Counts { get; set; }
        public required decimal[] RetailCosts { get; set; }
        public required decimal[] Allowances { get; set; }
        public required decimal[] VendorCosts { get; set; }
        public required decimal[] NetCosts { get; set; }
        public required bool Update { get; set; }
    }
}
