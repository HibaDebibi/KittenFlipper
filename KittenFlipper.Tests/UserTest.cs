using System.Collections.Generic;
using AutoMapper;
using Castle.Core.Logging;
using KittenFlipper.Contracts;
using KittenFlipper.Controllers;
using KittenFlipper.Entitites;
using KittenFlipper.Infrastructure;
using KittenFlipper.Infrastructure.Jwt;
using KittenFlipper.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace KittenFlipper.Tests
{
    public class UserTest
    {
        [Test]
        public void CreateUser()
        {
            // Arrange
            Mock<IUserService> userService = new Mock<IUserService>();
            User user = new User()
            {
                Id = 1,
                FirstName = "admin",
                LastName = "admin",
                Username = "admin",
            };
            userService.Setup(m => m.Create(user, "admin")).Returns(user);
            ;
            Mock<ILogger<UserController>> logger = new Mock<ILogger<UserController>>();
            Mock<IMapper> mapper = new Mock<IMapper>();
            var regModel = new RegisterModel() { FirstName = "admin", LastName = "admin", Username = "admin", Password = "admin" };
            var controller = new UserController(logger.Object, userService.Object, mapper.Object);

            // Act
            var result = controller.Register(regModel);

            // Assert
            Assert.True(result is OkResult);
            Assert.AreEqual(StatusCodes.Status200OK, ((OkResult)result).StatusCode);
        }
        [Test]
        public void UpdateUser()
        {
            // Arrange
            Mock<IUserService> userService = new Mock<IUserService>();
            User user = new User()
            {
                Id = 1,
                FirstName = "admin",
                LastName = "admin",
                Username = "admin",
            };
            userService.Setup(m => m.Update(user, "admin"));
            ;
            Mock<ILogger<UserController>> logger = new Mock<ILogger<UserController>>();
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();
            var updateModel = new UpdateModel() { FirstName = "admin2", LastName = "admin2", Username = "admin", Password = "admin" };
            var controller = new UserController(logger.Object, userService.Object, mapper);

            // Act
            var result = controller.Update(1,updateModel);

            // Assert
            Assert.True(result is OkResult);
            Assert.AreEqual(StatusCodes.Status200OK, ((OkResult)result).StatusCode);
        }

        [Test]
        public void GetAllUsers()
        {
            // Arrange
            Mock<IUserService> userService = new Mock<IUserService>();
            var list = new List<User>
                     {
                         new User() { Id = 1, FirstName = "admin", LastName = "admin", Username = "admin", PasswordHash = null, PasswordSalt = null },
                         new User() { Id = 2, FirstName = "admin2", LastName = "admin2", Username = "admin2", PasswordHash = null, PasswordSalt = null }
                         };
            userService.Setup(m => m.GetAll())
                     .Returns(list);
            Mock<ILogger<UserController>> logger = new Mock<ILogger<UserController>>();
            //Mock<IMapper> mapper = new Mock<IMapper>();

            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();
            var controller = new UserController(logger.Object, userService.Object, mapper);

            // Act
            var result = controller.GetAll();

            // Assert
            Assert.True(result is OkObjectResult);
            Assert.AreEqual(StatusCodes.Status200OK, ((OkObjectResult)result).StatusCode);
            Assert.AreEqual(2, ((List<UserModel>)(((OkObjectResult)result).Value)).Count);
            Assert.AreEqual("admin", ((List<UserModel>)(((OkObjectResult)result).Value))[0].FirstName);
            Assert.AreEqual("admin2", ((List<UserModel>)(((OkObjectResult)result).Value))[1].FirstName);
        }
    }
}