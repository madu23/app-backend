using Bilbayt.Application.DTOs;
using Bilbayt.Application.Interfaces;
using Bilbayt.Domain.Entities;
using Bilbayt.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bilbayt.Services.Implementations
{
    public class UserAccountService : IUserAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        public UserAccountService(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // business logic to register a new user on the application
        public async Task<BaseResponse<CreateUserResponse>> CreateUser(CreateAccountDTO model)
        {
            // check to ensure the user email does not already exist.
            var user = await _userManager.FindByEmailAsync(model.UserName);
            if (user != null)
            {
                return new BaseResponse<CreateUserResponse>
                {
                    Message = "User with existing email already exists",
                    Succeeded = false
                };
            }

            // check to ensure the username does not already exist.
            user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                if (user != null)
                {
                    return new BaseResponse<CreateUserResponse>
                    {
                        Message = "Username already exists",
                        Succeeded = false
                    };
                }
            }

            // if all check passed. Then try creating the user.
            var newUser = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = model.UserName,
                Email = model.UserName,
                FullName = model.FullName,
                EmailConfirmed = false
            };
            var createUserResult = await _userManager.CreateAsync(newUser, model.Password);
            if (createUserResult.Succeeded)
            {
                return new BaseResponse<CreateUserResponse>
                {
                    Message = "Registration is successful",
                    Succeeded = true
                };
            }
            return new BaseResponse<CreateUserResponse>
            {
                Message = String.Join(",", createUserResult.Errors.Select(err => err.Description)),
                Succeeded = true
            };
        }

        // business logic to generate token for user on login
        public async Task<BaseResponse<LoginResponse>> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var secret = _configuration["JWT:Secret"];
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(5),
                    //claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return new BaseResponse<LoginResponse>
                {
                    Succeeded = true,
                    ResultData = new LoginResponse
                    {
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo,
                        FullName = user.FullName
                    }
                };
            }
            return new BaseResponse<LoginResponse>
            {
                Succeeded = false,
                Message = "username or password incorrect"
            };
        }
    }
}
