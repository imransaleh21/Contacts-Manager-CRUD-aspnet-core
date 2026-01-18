using ContactsManager.Core.Enums;
using ContactsManager.Infrastructure.IdentityEntities;
using Microsoft.AspNetCore.Identity;
namespace ContactsManager.Infrastructure.Seeder
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            string[] roles =
            {
                UserRoleOptions.Admin.ToString(),
                UserRoleOptions.User.ToString()
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var assignableRole = new ApplicationRole
                    {
                        Name = role, // the Name is parent class IdentityRole property
                        NormalizedName = role.ToUpper() // the NormalizedName is parent class IdentityRole property
                    };
                    await roleManager.CreateAsync(assignableRole);
                }
            }
        }
    }
}
