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

    /*using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand("SELECT * FROM Employees", conn);
    using SqlDataReader reader = command.ExecuteReader();

    if (reader.HasRows)
    {
        while (reader.Read())
        {
            rows.Add($"{reader.GetInt32(0)}, {reader.GetString(1)}, {reader.GetString(2)}");
        }
    }

    return rows;
    */
})
.WithName("GetEmployee");
//.WithOpenApi();

app.MapPost("/Employees", async (Employee employee, MyDbContext dbContext) => {
    dbContext.Employees.Add(employee);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/entities/{employee.emp_no}", employee);
    /*using var conn = new SqlConnection(connectionString);
    conn.Open();

    var command = new SqlCommand(
        "INSERT INTO Persons (firstName, lastName) VALUES (@firstName, @lastName)",
        conn);

    command.Parameters.Clear();
    command.Parameters.AddWithValue("@emp_no", employee.emp_no);
    command.Parameters.AddWithValue("@firstName", employee.FirstName);
    command.Parameters.AddWithValue("@lastName", employee.LastName);

    using SqlDataReader reader = command.ExecuteReader();
    */
})
.WithName("CreateEmployee");
//.WithOpenApi();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();