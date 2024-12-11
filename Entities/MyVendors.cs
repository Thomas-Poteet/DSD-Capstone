using System.ComponentModel.DataAnnotations;


public class Vendor
{
    public required string vendor_id { get; set; }
    [Key]
    public required int vendor_no { get; set; }
    public required string name { get; set; }
    public required string address_1 { get; set; }
    public required string city { get; set; }
    public required string state { get; set; }
    public required string zip { get; set; }
    public required string phone { get; set; }
}