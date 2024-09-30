using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
        options.SlidingExpiration = true;
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

// Get password for employee username
app.MapGet("/Employees/{username}", async (MyDbContext dbContext, string username) => {
    var conn = await dbContext.Employees.FirstOrDefaultAsync(e => e.UserName == username);
    if (conn == null)
    {
        return Results.NotFound("Employee not found");
    }
    else{
        return Results.Ok(new
        {
            Password = conn.AdminPassword
        });
    }
})
.WithName("GetPasswordByUsername");

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
            Price = conn.normal_price
        });
    }
})
.WithName("GetProductByUPC");

app.MapGet("/VendorNames", async (MyDbContext dbContext) => {
    var conn = await dbContext.Vendors
        .Select(v => v.name)
        .ToListAsync();
})
.WithName("GetVendorNames");

app.MapGet("/Products", async (MyDbContext dbContext) => {
    var conn = await dbContext.Products.ToListAsync();
    return Results.Ok(conn);
});

app.MapPost("/Products", async (Product product, MyDbContext dbContext) => {
    dbContext.Products.Add(product);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/entities/{product.upc}", product);
})
.WithName("CreateProduct");

app.MapPost("/Vendors", async (Vendor vendor, MyDbContext dbContext) => {
    dbContext.Vendors.Add(vendor);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/entities/{vendor.vendor_no}", vendor);
})
.WithName("CreateVendor");

app.MapPost("/Employees", async (Employee employee, MyDbContext dbContext) => {
    dbContext.Employees.Add(employee);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/entities/{employee.emp_no}", employee);
})
.WithName("CreateEmployee");
//.WithOpenApi();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

// makes login default page
app.MapGet("/", (MyDbContext dbContext) => {
    return Results.Redirect("/Login");
});

app.Run();