public class Allowance
{
    public required int vendor_no { get; set; }
    public required string upc { get; set; }
    public required DateOnly start_date { get; set; }
    public required DateOnly  end_date { get; set; }
    public required decimal  discount_cost { get; set; }
}   