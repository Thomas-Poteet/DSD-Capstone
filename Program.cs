using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// For Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "DSD_Capstone API";
    config.Title = "DSD_Capstone API v1";
    config.Version = "v1";
});

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.SlidingExpiration = false;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
} else 
{
    // For Swagger
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "DSD_Capstone API";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });

}
app.UseHttpsRedirection();
//string connectionString = app.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")!;

app.MapGet("/Employees", async (MyDbContext dbContext) => {
    var employees = await dbContext.Employees.ToListAsync();
    return Results.Ok(employees);
})
.WithName("GetEmployee");
//.WithOpenApi();

//Get call to fetch the emp_FirstName and emp_LastName based off the name of the emp_no
app.MapGet("/Employees/{emp_no}", async (MyDbContext dbContext, int emp_no) => {
    var conn = await dbContext.Employees.FirstOrDefaultAsync(e => e.emp_no == emp_no);
    if (conn == null)
    {
        return Results.NotFound("Employee not found");
    }
    else{
        return Results.Ok(new
        {
            FirstName = conn.FirstName,
            LastName = conn.LastName
        });
    }
});

//Get call to fetch the cost of a product from the VendorsProducts table using a given UPC code and vendor_no
app.MapGet("/vendorsProducts/{upc}/{vendor_no}", async (MyDbContext dbContext, string upc, int vendor_no) => {
    var conn = await dbContext.VendorsProducts.FindAsync(vendor_no, upc);
    if (conn == null)
    {
        return Results.NotFound("Vendor product not found");
    }
    else{
        return Results.Ok(new
        {
            Cost = conn.cost
        });
    }
});


//Get call to fetch the vendor_no based off the name of the vendor
app.MapGet("/vendors/{vendorName}", async (MyDbContext dbContext, string vendorName) => {
    var conn = await dbContext.Vendors.FirstOrDefaultAsync(v => v.name == vendorName);
    if (conn == null)
    {
        return Results.NotFound("Vendor not found");
    }
    else{
        return Results.Ok(new
        {
            vendor_no = conn.vendor_no
        });
    }
});

//Get call to fetch the vendorName based off the vendor_no
app.MapGet("/VendorByNum/{vendor_no}", async (MyDbContext dbContext, int vendor_no) => {
    var conn = await dbContext.Vendors.FirstOrDefaultAsync(v => v.vendor_no == vendor_no);
    if (conn == null)
    {
        return Results.NotFound("Vendor not found");
    }
    else{
        return Results.Ok(new
        {
            name = conn.name
        });
    }
});


//Get call to fetch a product from the products table using a given UPC code
app.MapGet("/products/{upc}", async (MyDbContext dbContext, string upc) => {
    var conn = await dbContext.Products.FirstOrDefaultAsync(p => p.upc == upc);
    if (conn == null)
    {
        return Results.NotFound("Product not found");
    }
    else{
        return Results.Ok(new
        {
            UPC = conn.upc,
            Name = conn.description,
            Price = conn.normal_price,
            Department = conn.department
        });
    }
})
.WithName("GetProductByUPC");


app.MapGet("/departments/{dept_no}", async (MyDbContext dbContext, short dept_no) => {
    var conn = await dbContext.Departments.FirstOrDefaultAsync(d => d.dept_no == dept_no);
    if (conn == null)
    {
        return Results.NotFound("Department not found");
    }
    else{
        return Results.Ok(new
        {
            dept_name = conn.dept_name
        });
    }
});


//Get call to fetch all vendor names from vendor table to fill vendor list on invoices
app.MapGet("/VendorNames", async (MyDbContext dbContext) => {
    var conn = await dbContext.Vendors
        .Select(v => v.name)
        .ToListAsync();
})
.WithName("GetVendorNames");


//Get call to fetch all Products from the Products table
app.MapGet("/Products", async (MyDbContext dbContext) => {
    var conn = await dbContext.Products.ToListAsync();
    return Results.Ok(conn);
});

//Get call to fetch an Invoice from InvoiceID
app.MapGet("/Invoices/{InvoiceID}", async (MyDbContext dbContext, string InvoiceID) => {
    var conn = await dbContext.Invoices.FindAsync(InvoiceID);
    if (conn == null)
    {
        return Results.NotFound("Invoice not found");
    }
    else{
        return Results.Ok(new
        {
            InvoiceID = conn.InvoiceID,
            Date = conn.Date,
            emp_no = conn.emp_no,
            vendor_no = conn.vendor_no,
            vendor_total = conn.vendor_total
        });
    }
})
.WithName("GetInvoiceByID");


//get call to fetch any allowances for a product by current date
app.MapGet("/Allowances/{upc}/{vendor_no}", async (MyDbContext dbContext, string upc, int vendor_no) => {
    var currentDate = DateTime.Now.Date;
    var conn = await dbContext.Allowances
        .Where(a => currentDate >= a.start_date && currentDate <= a.end_date && upc == a.upc && vendor_no == a.vendor_no)
        .ToListAsync();
    if (conn == null)
    {
        return Results.NotFound("No allowances");
    }
    else{
        return Results.Ok(conn);
    }
});

//Get call to fetch Invoices from a date range
app.MapGet("/InvoicesByDate/{start}/{end}", async (MyDbContext dbContext, DateOnly start, DateOnly end) => {
    var invoices = await dbContext.Invoices
        .Where(i => i.Date >= start && i.Date <= end)
        .ToListAsync();
        
    if (invoices == null )
    {
        return Results.NotFound("Invoices not found");
    }

    // Return a list of invoices
    return Results.Ok(invoices.Select(conn => new
    {
        InvoiceID = conn.InvoiceID,
        Date = conn.Date,
        emp_no = conn.emp_no,
        vendor_no = conn.vendor_no,
        vendor_total = conn.vendor_total,
        retail_total = conn.retail_total,
        gross = conn.gross,
        count_total = conn.count_total
    }));
})
.WithName("GetInvoicesByDate");

// Get Invoices by vendor_no
app.MapGet("/InvoicesByVendor/{vendor_no}", async (MyDbContext dbContext, int vendor_no) => {
    var invoices = await dbContext.Invoices
        .Where(i => i.vendor_no == vendor_no)
        .ToListAsync();
        
    if (invoices == null )
    {
        return Results.NotFound("Invoices not found");
    }

    // Return a list of invoices
    return Results.Ok(invoices.Select(conn => new
    {
        InvoiceID = conn.InvoiceID,
        Date = conn.Date,
        emp_no = conn.emp_no,
        vendor_no = conn.vendor_no,
        vendor_total = conn.vendor_total,
        retail_total = conn.retail_total,
        gross = conn.gross,
        count_total = conn.count_total
    }));
})
.WithName("GetInvoicesByVendor");



//Post to create a new product
app.MapPost("/Products", async (Product product, MyDbContext dbContext) => {
    dbContext.Products.Add(product);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/entities/{product.upc}", product);
})
.WithName("CreateProduct");


//Post to create a new vendor
app.MapPost("/Vendors", async (Vendor vendor, MyDbContext dbContext) => {
    dbContext.Vendors.Add(vendor);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/entities/{vendor.vendor_no}", vendor);
})
.WithName("CreateVendor");


//Post to create a new VendorProduct
app.MapPost("/CreateVendorProduct", async (VendorProduct vendorProduct, MyDbContext dbContext) => {
    dbContext.VendorsProducts.Add(vendorProduct);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/entities/{vendorProduct.vendor_no}", vendorProduct);
});


//Post to create a new employee 
app.MapPost("/Employees", async (Employee employee, MyDbContext dbContext) => {
    dbContext.Employees.Add(employee);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/entities/{employee.emp_no}", employee);
})
.WithName("CreateEmployee");

//Post to create a new invoice
app.MapPost("/Invoices", async (Invoice invoice, MyDbContext dbContext) => {
    dbContext.Invoices.Add(invoice);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/entities/{invoice.InvoiceID}", invoice);
})
.WithName("CreateInvoice");
//.WithOpenApi();


app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();