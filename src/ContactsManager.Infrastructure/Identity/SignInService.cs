using ContactsManager.Core.Helpers;
using ContactsManager.Core.IdentityContracts;

namespace ContactsManager.Infrastructure.Identity
{
    public class SignInService : ISignInService
    {
        public async Task<Result<Guid>> SignInUser(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
