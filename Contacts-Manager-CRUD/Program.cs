using Contacts_Manager_CRUD.Filters.ActionFilters;
using Contacts_Manager_CRUD.Middleware;
using Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Repository;
using RepositoryContracts;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);

// Configuring Serilog as the logging provider
builder.Host.UseSerilog((HostBuilderContext context,
    IServiceProvider services,
    LoggerConfiguration configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration) // Read configuration from appsettings.json by using IConfiguration
    .ReadFrom.Services(services); // Read current services and make them available to Serilog
});

builder.Services.AddControllersWithViews(options =>
{
    // Adding a global action filter to add custom headers to responses
    options.Filters.Add(new ResponseHeaderActionFilter("Global-Custom-key", "Custom-value", 3));
});


// Registering services for dependency injection
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();

if (builder.Environment.IsEnvironment("Testing") == false)
{
    // Registering DbContext with SQL Server
    builder.Services.AddDbContext<PersonsDbContext>
    (options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // Connection string from appsettings.json
    });
    // Setting the EPPlus license context for non-commercial use to generate Excel files
    ExcelPackage.License.SetNonCommercialPersonal("Imran88");
    // Configuring Rotativa for PDF generation from views(HTML)
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}
// Building the app
var app = builder.Build();

if (builder.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
{
    // This is the built-in exception handling middleware of ASP.NET Core at the top of the middleware pipeline
    app.UseExceptionHandler("/error"); // Redirecting to the Error action method of GeneralPurposeController for unhandled exceptions
    // Custom exception handling middleware
    app.UseExceptionHandlingMiddleware();
}

// Enabling static files middleware
app.UseStaticFiles();
// Enabling routing middleware
app.UseRouting();
// Enabling endpoint routing middleware
app.MapControllers();

app.Run();

// To make the Compiler generate the Program class as partial for integration tests
public partial class Program { }
