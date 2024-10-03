using System.ComponentModel.DataAnnotations;


public class Invoice
{
    [Key]
    public required char InvoiceID { get; set; }
    
    public required int vendor_no { get; set; }

    public required short emp_no { get; set; }

    public required DateTime Date { get; set; }

}