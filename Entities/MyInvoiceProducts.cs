public class InvoiceProduct
{
    public required string InvoiceID { get; set; }
    public required int vendor_no { get; set; }
    public required string upc { get; set; }
    public required int count { get; set; }
    public required decimal retail_cost { get; set; }
    public required decimal allowances { get; set; }
    public required decimal vendor_cost { get; set; }
    public required decimal net_cost { get; set; }
    // define a foreign key relationship 
    public required Invoice Invoice { get; set; }
}