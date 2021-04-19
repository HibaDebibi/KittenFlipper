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
   public class KittenFlipperTest
    {

        [Test]
        public void ShouldHaveValueBetween1and16FlipImage()
        {
            // Arrange
            Mock<IKittenFlipperService> kittenService = new Mock<IKittenFlipperService>();
            var controller = new ImageFlipperController(kittenService.Object);

            // Act
            var result = controller.GetAsync(17).Result;

            // Assert
            Assert.True(result is ContentResult);
            Assert.AreEqual("Please enter rotation type between 1 and 16", ((ContentResult)result).Content);
        }
    }
}
