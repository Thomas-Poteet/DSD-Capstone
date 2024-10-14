public class InvoiceProduct
{
    public required string InvoiceID { get; set; }
    public required string upc { get; set; }
    public required int count { get; set; }

    // define a foreign key relationship 
    public required Invoice Invoice { get; set; }
}