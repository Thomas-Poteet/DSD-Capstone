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


// Just to see how Swagger works
app.MapGet("/hello", () => "Hello World!");
app.MapGet("/helloyou", (string strName) => "Hello " + strName + "!");

app.MapGet("/Employees", async (MyDbContext dbContext) => {
    var employees = await dbContext.Employees.ToListAsync();
    return Results.Ok(employees);
})
.WithName("GetEmployee");
//.WithOpenApi();

app.MapGet("/ProuctsByUPC", async (string upc, MyDbContext dbContext) => {
    var conn = await dbContext.Products.FindAsync(upc);
    if (conn == null)
    {
        return Results.NotFound();
    }
    else{
        return Results.Ok(conn);
    }
})
.WithName("GetProductByUPC");

app.MapGet("/VendorNames", async (MyDbContext dbContext) => {
    var conn = await dbContext.Vendors
        .Select(v => v.name)
        .ToListAsync();
})
.WithName("GetVendorNames");

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();