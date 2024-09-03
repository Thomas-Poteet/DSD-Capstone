using System.ComponentModel.DataAnnotations;


public class Department
{
    [Key]
    public required short dept_no { get; set; }
    public required string dept_name { get; set; }
    public required string LastName { get; set; }
    public required bool EmpActive { get; set; }
}   