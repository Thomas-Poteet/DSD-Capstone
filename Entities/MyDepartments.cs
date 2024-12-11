using System.ComponentModel.DataAnnotations;


public class Department
{
    public required short dept_no { get; set; }
    public required short dept_sub { get; set; }
    public required string dept_name { get; set; }
    //public required string LastName { get; set; }
    //public required bool EmpActive { get; set; }
}   