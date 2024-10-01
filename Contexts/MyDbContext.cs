using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }  

    public DbSet<Product> Products { get; set; }

    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<VendorProduct> VendorsProducts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //sets the VendorProduct entity's primary key as composite
        modelBuilder.Entity<VendorProduct>()
            .HasKey(vp => new { vp.vendor_no, vp.upc });  // Composite primary key

        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Department> Departments { get; set; }
}
