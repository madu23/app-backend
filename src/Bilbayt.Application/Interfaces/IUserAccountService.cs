using Bilbayt.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bilbayt.Application.Interfaces
{
    public interface IUserAccountService
    {
        Task<BaseResponse<CreateUserResponse>> CreateUser(CreateAccountDTO model);
        Task<BaseResponse<LoginResponse>> Login(LoginDTO model);
    }
}
