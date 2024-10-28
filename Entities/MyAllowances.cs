using System.ComponentModel.DataAnnotations;

public class Allowance
{
    public required int vendor_no { get; set; }
    public required string upc { get; set; }
    public required DateTime start_date { get; set; }
    public required DateTime  end_date { get; set; }
    public required decimal  discount_cost { get; set; }
}   