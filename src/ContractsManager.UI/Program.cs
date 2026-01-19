using Contacts_Manager_CRUD.Filters.ActionFilters;
using Contacts_Manager_CRUD.Middleware;
using ContactsManager.Core.IdentityContracts;
using ContactsManager.Infrastructure.IdentityEntities;
using ContactsManager.Infrastructure.Identity;
using ContactsManager.Infrastructure.Seeder;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Repository;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using Services;
using Microsoft.AspNetCore.Authorization;

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


// Registering Repositories and Services in the DI container
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
// Registering Person services
builder.Services.AddScoped<IPersonsGetterService, PersonsGetterServiceReducedExcelColumns>();
builder.Services.AddScoped<PersonsGetterService, PersonsGetterService>();
builder.Services.AddScoped<IPersonsSorterService, PersonsSorterService>();
builder.Services.AddScoped<IPersonsAdderService, PersonsAdderService>();
builder.Services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();
builder.Services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
// Registering Identity services
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<ISignInService, SignInService>();
builder.Services.AddScoped<ILogOutService, LogOutService>();

if (builder.Environment.IsEnvironment("Testing") == false)
{
    // Registering DbContext with SQL Server
    builder.Services.AddDbContext<PersonsDbContext>
    (options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // Connection string from appsettings.json
    });

    // Configuring Identity with ApplicationUser and ApplicationRole
    builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
        options.Password.RequiredLength = 3;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireDigit = false;
    })
        .AddEntityFrameworkStores<PersonsDbContext>()
        .AddDefaultTokenProviders()
        .AddUserStore<UserStore<ApplicationUser, ApplicationRole, PersonsDbContext, Guid>>()
        .AddRoleStore<RoleStore<ApplicationRole, PersonsDbContext, Guid>>();

    // Setting the EPPlus license context for non-commercial use to generate Excel files
    ExcelPackage.License.SetNonCommercialPersonal("Imran88");
    // Configuring Rotativa for PDF generation from views(HTML)
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}

builder.Services.AddAuthorization(options =>
{// Setting a fallback policy that requires authenticated users by default for all endpoints
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser().Build();
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Redirecting to Login action method of AccountController for unauthenticated users
    //options.LogoutPath = "/Account/LogOut"; // Redirecting to LogOut action method of AccountController for logging out
    //options.AccessDeniedPath = "/Account/AccessDenied"; // Redirecting to AccessDenied action method of AccountController for unauthorized access attempts
});

// Building the app
var app = builder.Build();

// Seed roles
if (builder.Environment.IsEnvironment("Testing") == false)
{
    using (var scope = app.Services.CreateScope())
    {
        // Getting the RoleManager service to seed roles
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        await RoleSeeder.SeedAsync(roleManager);
    }
}

if (builder.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
{
    // This is the built-in exception handling middleware of ASP.NET Core at the top of the middleware pipeline
    app.UseExceptionHandler("/error"); // Redirecting to the Error action method of GeneralPurposeController for unhandled exceptions
    // Custom exception handling middleware
    app.UseExceptionHandlingMiddleware();
}

// Enforcing HTTP Strict Transport Security (HSTS) for secure communication.
// It adds the Strict-Transport-Security header to responses and instructs browsers to only use HTTPS for future requests to the site
app.UseHsts();
// Enforcing HTTPS redirection middleware
app.UseHttpsRedirection();

// Enabling static files middleware
app.UseStaticFiles();

// Enabling routing middleware
app.UseRouting();
// Enabling authentication middleware
app.UseAuthentication();
// Enabling authorization middleware, it ensures user is authorized to access secure resources
app.UseAuthorization();
// Enabling endpoint routing middleware
app.MapControllers();

app.Run();

// To make the Compiler generate the Program class as partial for integration tests
public partial class Program { }
