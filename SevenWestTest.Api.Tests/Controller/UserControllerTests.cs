using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SevenWestTest.Api.Controllers;
using SevenWestTest.Api.Services;
using SevenWestTest.Dto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SevenWestTest.Api.Tests.Controller
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController _userController;
        private IUserService _userService;
        private ILogger<UserController> _logger;

        [SetUp]
        public void SetUp()
        {
            _userService = Substitute.For<IUserService>();
            _logger = Substitute.For<ILogger<UserController>>();
            _userController = new UserController(_userService, _logger);
        }


        [Test]
        public async Task Get_Should_Return_Users_Succesfully()
        {
            var users = new List<User>()
            {
                new User { First = "John" , Last = "Doe" , Age = 26, Gender = "M" ,Id = 1}
            };

            _userService.GetAllUsers().Returns(users);
            var result = await _userController.Get() as ObjectResult;
            var userResult = result.Value as List<User>;
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf(typeof(OkObjectResult), result);
            Assert.AreEqual(userResult.Count, 1);
            var user = userResult.First();
            Assert.AreEqual(user.First, "John");
            Assert.AreEqual(user.Id, 1);
        }

        [Test]
        public async Task Get_Should_Return_NoContent_Response_When_UserApi_Returns_Unsucessfull_Response()
        {
            var users = new List<User>();

            _userService.GetAllUsers().Returns(users);
            var result = await _userController.Get() as NoContentResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(204, result.StatusCode);
            Assert.IsInstanceOf(typeof(NoContentResult), result);
        }

        [Test]
        public async Task Get_Should_Return_InternalServerError_Response_InCaseOf_Exception()
        {

            _userService.When(x => x.GetAllUsers()).Do(x => { throw new Exception(); });
            var result = await _userController.Get() as StatusCodeResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);
            Assert.IsInstanceOf(typeof(StatusCodeResult), result);
        }
    }
}
