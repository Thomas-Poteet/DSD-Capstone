using System.ComponentModel.DataAnnotations;


public class SiteSettings
{
    [Key]
    public required int SettingID { get; set; }
    public required string SettingKey { get; set; }
    public required string SettingValue { get; set; }
    public required string SettingType { get; set; }
    public required DateTime LastUpdated { get; set; }
}   