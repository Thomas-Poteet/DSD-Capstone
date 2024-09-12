using System.ComponentModel.DataAnnotations;


public class Product
{
    [Key]
    public required string upc { get; set; }
    public required string description { get; set; }
    public required decimal normal_price { get; set; }
    public required short pricemethod { get; set; }
    public required decimal groupprice { get; set; }
    public required short quantity { get; set; }
    public required short department { get; set; }
    public required int vendor { get; set; }
}