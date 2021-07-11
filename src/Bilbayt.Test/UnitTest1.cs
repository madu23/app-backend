using Bilbayt.API.Controllers;
using Bilbayt.Application.DTOs;
using Bilbayt.Application.Interfaces;
using Bilbayt.Application.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Bilbayt.Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Login_Successful_Should_Return_Username()
        {

            var testUser = new LoginDTO
            {
                UserName = "test@bilbayt.com",
                Password = "testP@ss2"
            };
            // Arrange
            var userService = new Mock<IUserAccountService>();
            userService.Setup(repo => repo.Login(testUser)).ReturnsAsync(LoginResult(testUser));

            var notificationSvc = new Mock<INotificationService>();
            notificationSvc.Setup(notify => notify.SendEmailAsync("no-reply@bilbayt.com", testUser.UserName, "New User", "Welcome")).ReturnsAsync(true);
            var settingOption = new Mock<IOptions<SendGridSetting>>();
            var controller = new AccountController(userService.Object, notificationSvc.Object, settingOption.Object);

            //Act
            var result = await controller.Login(testUser);

            // Assert
            var OkRequestResult = Assert.IsType<OkObjectResult>(result);

            var okResultData = Assert.IsType<BaseResponse<LoginResponse>>(
        OkRequestResult.Value);

            Assert.Equal("TEST USER", okResultData.ResultData.FullName);
        }

        [Fact]
        public async Task Login_NotSuccessful_Should_Return_BadResquest()
        {

            var testUser = new LoginDTO
            {
                UserName = "invalidUser@bilbayt.com",
                Password = "testP@ss2"
            };
            // Arrange
            var userService = new Mock<IUserAccountService>();
            userService.Setup(repo => repo.Login(testUser)).ReturnsAsync(LoginResult(testUser));

            var notificationSvc = new Mock<INotificationService>();
            var settingOption = new Mock<IOptions<SendGridSetting>>();
            var controller = new AccountController(userService.Object, notificationSvc.Object, settingOption.Object);

            //Act
            var result = await controller.Login(testUser);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_NotSuccessful_Should_Return_InvalidUsernameOrPassword()
        {

            var testUser = new LoginDTO
            {
                UserName = "invalidUser@bilbayt.com",
                Password = "testP@ss2"
            };
            // Arrange
            var userService = new Mock<IUserAccountService>();
            userService.Setup(repo => repo.Login(testUser)).ReturnsAsync(LoginResult(testUser));

            var notificationSvc = new Mock<INotificationService>();
            var settingOption = new Mock<IOptions<SendGridSetting>>();
            //notificationSvc.Setup(notify => notify.SendEmailAsync("no-reply@bilbayt.com", testUser.UserName, "New User", "Welcome")).ReturnsAsync(true);
            var controller = new AccountController(userService.Object, notificationSvc.Object, settingOption.Object);

            //Act
            var result = await controller.Login(testUser);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;

            var badRequestResultData = Assert.IsType<BaseResponse<LoginResponse>>(
        badRequestResult.Value);

            Assert.Equal("Inavlid username or password", badRequestResultData.Message);
        }

        private BaseResponse<LoginResponse> LoginResult(LoginDTO loginModel)
        {
            // do some logic
            if(loginModel.UserName == "test@bilbayt.com" && loginModel.Password == "testP@ss2")
            {
                return new BaseResponse<LoginResponse>
                {
                    Message = "Login was sucessful",
                    Succeeded = true,
                    ResultData = new LoginResponse { AccessToken = "test token", Expiration = DateTime.Now, FullName = "TEST USER" }
                };
            }
            else
            {
                return new BaseResponse<LoginResponse>
                {
                    Message = "Inavlid username or password",
                    ResultData = null,
                    Succeeded = false
                };
            }
        }
    }
}
