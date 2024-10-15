using System.ComponentModel.DataAnnotations;

public class Invoice
{
    [Key]
    public required string InvoiceID { get; set; }
    public required short emp_no { get; set; }
    public required int vendor_no { get; set; }
    public required DateOnly Date { get; set; }
    public required decimal vendor_total { get; set; }

    // define a foreign key relationship
    public required List<InvoiceProduct> InvoiceProducts { get; set; }
}