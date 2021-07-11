using Bilbayt.Application.DTOs;
using Bilbayt.Application.Interfaces;
using Bilbayt.Application.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bilbayt.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public readonly IUserAccountService _userAccountService;
        public readonly INotificationService _notificationService;
        private readonly SendGridSetting _setting;
        public AccountController(IUserAccountService accountService, INotificationService notificationService, IOptions<SendGridSetting> options)
        {
            _userAccountService = accountService;
            _notificationService = notificationService;
            _setting = options.Value;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(BaseResponse<CreateUserResponse>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] CreateAccountDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var registerResult = await _userAccountService.CreateUser(model);
            if (!registerResult.Succeeded)
            {
                return BadRequest(registerResult);
            }
            // send email to user
            var sendMail = await _notificationService.SendEmailAsync(_setting.FromEmail, model.UserName, "New Registration", "Registration was successful");
            return Ok(registerResult);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(BaseResponse<LoginResponse>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var loginResult = await _userAccountService.Login(model);
            if (!loginResult.Succeeded)
            {
                return BadRequest(loginResult);
            }
            return Ok(loginResult);
        }
    }
}
