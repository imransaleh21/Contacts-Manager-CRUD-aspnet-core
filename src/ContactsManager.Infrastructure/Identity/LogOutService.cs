using ContactsManager.Core.IdentityContracts;
using ContactsManager.Infrastructure.IdentityEntities;
using Microsoft.AspNetCore.Identity;

namespace ContactsManager.Infrastructure.Identity
{
    public class LogOutService : ILogOutService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        public LogOutService(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
