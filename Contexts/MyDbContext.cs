using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    public required DbSet<Employee> Employees { get; set; }
    public required DbSet<Product> Products { get; set; }
    public required DbSet<SiteSettings> SiteSettings { get; set; }
    public required DbSet<Vendor> Vendors { get; set; }
    public required DbSet<VendorProduct> VendorsProducts { get; set; }
    public required DbSet<Allowance> Allowances { get; set; }
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
        
        modelBuilder.Entity<Allowance>()
            .Property(a => a.discount_cost)
            .HasColumnType("money");

        modelBuilder.Entity<InvoiceProduct>()
            .Property(i => i.retail_cost)
            .HasColumnType("money");
        
        modelBuilder.Entity<InvoiceProduct>()
            .Property(i => i.allowances)
            .HasColumnType("money");

        modelBuilder.Entity<InvoiceProduct>()
            .Property(i => i.vendor_cost)
            .HasColumnType("money");

        modelBuilder.Entity<InvoiceProduct>()
            .Property(i => i.net_cost)
            .HasColumnType("money");

        //sets the Department entity's primary key as composite
        modelBuilder.Entity<Department>()
            .HasKey(d => new { d.dept_no, d.dept_sub });

        modelBuilder.Entity<Invoice>()
            .HasKey(i => new { i.InvoiceID, i.vendor_no });
        
        modelBuilder.Entity<ProductGroupDetail>()
            .HasKey(pgd => new { pgd.PGCode, pgd.UPC });

        base.OnModelCreating(modelBuilder);
    }
    public required DbSet<Department> Departments { get; set; }
    public required DbSet<Invoice> Invoices { get; set; }
    public required DbSet<InvoiceProduct> InvoiceProducts { get; set; }
    public required DbSet<ProductGroupDetail> ProductGroupDetail { get; set; }

}
