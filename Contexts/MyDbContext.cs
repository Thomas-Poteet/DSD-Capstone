using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Product> Products { get; set; }

    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<VendorProduct> VendorsProducts { get; set; }
    public DbSet<Allowance> Allowances { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //sets the VendorProduct entity's primary key as composite
        modelBuilder.Entity<VendorProduct>()
            .HasKey(vp => new { vp.vendor_no, vp.upc });  // Composite primary key

        modelBuilder.Entity<InvoiceProduct>()
            .HasKey(ip => new { ip.InvoiceID, ip.upc, ip.vendor_no });

        //Allowances primary composite key
        modelBuilder.Entity<Allowance>()
            .HasKey(a => new { a.vendor_no, a.upc, a.start_date, a.end_date });
            
        modelBuilder.Entity<Product>()
            .Property(p => p.normal_price)
            .HasColumnType("money");
        
        modelBuilder.Entity<Product>()
            .Property(p => p.groupprice)
            .HasColumnType("money");

        modelBuilder.Entity<VendorProduct>()
            .Property(p => p.cost)
            .HasColumnType("money");

        modelBuilder.Entity<Invoice>()
            .Property(i => i.vendor_total)
            .HasColumnType("money");
   
        modelBuilder.Entity<Invoice>()
            .Property(i => i.retail_total)
            .HasColumnType("money");

        //sets the Department entity's primary key as composite
        modelBuilder.Entity<Department>()
            .HasKey(d => new { d.dept_no, d.dept_sub });

        modelBuilder.Entity<Invoice>()
            .HasKey(i => new { i.InvoiceID, i.vendor_no });

        base.OnModelCreating(modelBuilder);
    }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceProduct> InvoiceProducts { get; set; }
}
