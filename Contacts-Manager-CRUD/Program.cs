using Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
// Registering services for dependency injection
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();

// Registering DbContext with SQL Server
builder.Services.AddDbContext<PersonsDbContext>
    (options =>{
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // Connection string from appsettings.json
    });
// Setting the EPPlus license context
ExcelPackage.License.SetNonCommercialPersonal("<Your Name>");
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
// Configuring Rotativa for PDF generation from views(HTML)
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath:"Rotativa");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
