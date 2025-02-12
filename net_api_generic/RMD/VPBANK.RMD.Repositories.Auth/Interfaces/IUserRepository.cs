using System.Collections.Generic;
using System.Threading.Tasks;
using VPBANK.RMD.Data.Auth;
using VPBANK.RMD.Data.Auth.Entities.POCOs;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.Repositories.Auth.Interfaces
{
    public interface IUserRepository : IRepository<AuthContext, User, int>
    {
    }
}
