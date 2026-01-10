using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace xUnitTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                // For testing, use the in-memory database context not the SQL Server one
                services.AddDbContext<PersonsDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryPersonsTestDb");
                });
            });
        }
    }
}
