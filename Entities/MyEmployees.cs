using System.ComponentModel.DataAnnotations;


public class Employee
{
    [Key]
    public required short emp_no { get; set; }
    public required int AdminPassword { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required bool EmpActive { get; set; }
    public required string UserName { get; set; }
}   