using System.ComponentModel.DataAnnotations;


public class VendorProduct
{
    [Key]
    public required int vendor_no { get; set; }
    public required string upc { get; set; }
    public required decimal cost { get; set; }
}