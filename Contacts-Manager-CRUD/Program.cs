using ServiceContracts;
using Services;
using Entities;
using Microsoft.EntityFrameworkCore;

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
