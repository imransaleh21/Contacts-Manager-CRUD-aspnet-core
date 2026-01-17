using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.IdentityContracts
{
    public interface ILogOutService
    {
        Task LogOut();
    }
}
