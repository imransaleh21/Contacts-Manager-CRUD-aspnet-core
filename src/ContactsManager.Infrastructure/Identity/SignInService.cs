using ContactsManager.Core.DTO;
using ContactsManager.Core.Helpers;
using ContactsManager.Core.IdentityContracts;
using ContactsManager.Infrastructure.IdentityEntities;
using Microsoft.AspNetCore.Identity;

namespace ContactsManager.Infrastructure.Identity
{
    public class SignInService : ISignInService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        public SignInService(SignInManager<ApplicationUser> signInManager)
        {
           _signInManager = signInManager;
        }
        public async Task<Result<Guid>> SignInUser(LoginDTO loginDTO)
        {
            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return Result<Guid>.Success(Guid.NewGuid());
            }
            else
            {
                return Result<Guid>.Failure(new List<string> { "Invalid login attempt." });
            }
        }
    }
}
