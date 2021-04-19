using AutoMapper;
using Castle.Core.Logging;
using KittenFlipper.Contracts;
using KittenFlipper.Controllers;
using KittenFlipper.Entitites;
using KittenFlipper.Infrastructure.Jwt;
using KittenFlipper.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace KittenFlipper.Tests
{
    public class LoginTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldNotAcceptInvalidUser()
        {
            // Arrange
            Mock<IUserService> userService = new Mock<IUserService>();
            userService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                      ;
            Mock<ILogger<LoginController>> logger = new Mock<ILogger<LoginController>>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            var loginModel = new LoginModel() { UserName = "", Password = "" };
            var controller = new LoginController(logger.Object, userService.Object, new TokenManagement() { Secret = "1234567890123456789" });

            // Act
            var result = controller.Login(loginModel) ;

            // Assert
            Assert.True(result is BadRequestObjectResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestObjectResult)result).StatusCode);
            Assert.AreEqual(((BadRequestObjectResult)result).Value.ToString(),
                      "{ message = Username or password is incorrect }");

        }

        [Test]
        public void LoginUser()
        {
            // Arrange
            Mock<IUserService> userService = new Mock<IUserService>();
            userService.Setup(m => m.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                     .Returns(new User() { Id=1,FirstName="admin",LastName="admin", Username="admin",PasswordHash=null, PasswordSalt=null }) ;
            Mock<ILogger<LoginController>> logger = new Mock<ILogger<LoginController>>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            var loginModel = new LoginModel() { UserName = "admin", Password = "admin" };
            var controller = new LoginController(logger.Object, userService.Object, new TokenManagement() { Secret = "1234567890123456789" });

            // Act
            var result = controller.Login(loginModel);

            // Assert
            Assert.True(result is OkObjectResult);
            Assert.AreEqual(StatusCodes.Status200OK, ((OkObjectResult)result).StatusCode);
        }
    }
}