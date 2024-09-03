using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }  // Define your DbSets (tables)

    //public DbSet<Department> Departments { get; set; }
}
