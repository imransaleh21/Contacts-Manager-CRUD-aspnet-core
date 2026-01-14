using ContactsManager.Core.DTO;
using ContactsManager.Core.Helpers;
using ContactsManager.Core.IdentityContracts;
using ContactsManager.Infrastructure.IdentityEntities;
using Microsoft.AspNetCore.Identity;

namespace ContactsManager.Infrastructure.Identity
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public RegisterService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<Result<Guid>> RegisterUser(RegisterDTO registerDTO)
        {
            ApplicationUser user = new()
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                PersonNamme = registerDTO.PersonName
            };
            IdentityResult identityResult = await _userManager.CreateAsync(user, registerDTO.Password);
            if (identityResult.Succeeded)
            {
              return Result<Guid>.Success(user.Id);
            }
            else
            {
                var errors = identityResult.Errors.Select(e => e.Description);
                return Result<Guid>.Failure(errors);
            }
        }
    }
}
