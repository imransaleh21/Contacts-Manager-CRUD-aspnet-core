using ContactsManager.Core.DTO;
using ContactsManager.Core.Enums;
using ContactsManager.Core.Helpers;
using ContactsManager.Core.IdentityContracts;
using ContactsManager.Infrastructure.IdentityEntities;
using Microsoft.AspNetCore.Identity;

namespace ContactsManager.Infrastructure.Identity
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public RegisterService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<Result<Guid>> RegisterUser(RegisterDTO registerDTO, bool signInAutomatically = true)
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
                // Assign role to user
                await AssignRoleToUser(user, registerDTO.Role);

                if (signInAutomatically)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                return Result<Guid>.Success(user.Id);
            }
            else
            {
                var errors = identityResult.Errors.Select(e => e.Description);
                return Result<Guid>.Failure(errors);
            }
        }
        /// <summary>
        /// Assigns a role to the specified user.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        private async Task AssignRoleToUser(ApplicationUser user, UserRoleOptions role)
        {
            if(role == UserRoleOptions.Admin)
            {
                await _userManager.AddToRoleAsync(user, UserRoleOptions.Admin.ToString());
            }
            else
            {
                await _userManager.AddToRoleAsync(user, UserRoleOptions.User.ToString());
            }
        }
    }
}
