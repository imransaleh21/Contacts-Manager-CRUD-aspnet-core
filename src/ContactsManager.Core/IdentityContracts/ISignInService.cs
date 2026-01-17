using ContactsManager.Core.DTO;
using ContactsManager.Core.Helpers;

namespace ContactsManager.Core.IdentityContracts
{
    public interface ISignInService
    {
        Task<Result<Guid>> SignInUser(LoginDTO loginDTO);
    }
}
