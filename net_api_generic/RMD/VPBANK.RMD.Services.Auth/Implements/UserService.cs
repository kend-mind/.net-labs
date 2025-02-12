using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPBANK.RMD.Data.Auth;
using VPBANK.RMD.Data.Auth.Entities.POCOs;
using VPBANK.RMD.EFCore;
using VPBANK.RMD.EFCore.Entities.Commons;
using VPBANK.RMD.EFCore.Generics;
using VPBANK.RMD.Repositories.Auth.Interfaces;
using VPBANK.RMD.Services.Auth.Interfaces;

namespace VPBANK.RMD.Services.Auth.Implements
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork<AuthContext> _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UserService(IUnitOfWork<AuthContext> unitOfWork, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
