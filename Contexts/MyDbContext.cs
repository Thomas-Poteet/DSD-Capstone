using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }  // Define your DbSets (tables)

    public DbSet<Product> Products { get; set; }

    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<VendorProduct> VendorsProducts { get; set; }
    public DbSet<Department> Departments { get; set; }
}
