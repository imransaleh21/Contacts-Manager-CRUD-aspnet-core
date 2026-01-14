using ContactsManager.Core.DTO;
using ContactsManager.Core.Helpers;

namespace ContactsManager.Core.IdentityContracts
{
    public interface IRegisterService
    {
        Task<Result<Guid>> RegisterUser(RegisterDTO registerDTO);
    }
}
