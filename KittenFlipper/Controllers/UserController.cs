using System;
using System.Collections.Generic;
using AutoMapper;
using KittenFlipper.Contracts;
using KittenFlipper.Entitites;
using KittenFlipper.Infrastructure.BasicAuth;
using KittenFlipper.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KittenFlipper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly IUserService _userService;

        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }
    
        /// <summary>
        /// Register a user : fill in the model
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                _userService.Create(user, model.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get All users without Authentication
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        /// <summary>
        /// Get a user by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        /// <summary>
        /// Update a user by id
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;

            try
            {
                // update user 
                _userService.Update(user, model.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }


        /// <summary>
        /// delete a user by id
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }

        /// <summary>
        /// API requires JWT auth
        /// </summary>
        /// <returns></returns>
        [HttpGet("jwt")]
        [Authorize]
        public IActionResult GetAllUsersJwtAuth()
        {
            var username = User.Identity.Name;
            _logger.LogInformation($"User [{username}] is visiting jwt auth with token {1}");
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        /// <summary>
        /// API requires Basic auth
        /// </summary>
        /// <returns></returns>
        [HttpGet("basic")]
        [BasicAuth] // You can optionally provide a specific realm --> [BasicAuth("my-realm")]
        public IActionResult GetAllUsersBasicAuth()
        {
            _logger.LogInformation("basic auth");
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        [HttpGet("basic-logout")]
        [BasicAuth]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult BasicAuthLogout()
        {
            _logger.LogInformation("basic auth logout");
            // NOTE: there's no good way to log out basic authentication. This method is a hack.
            HttpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"My Realm\"";
            return new UnauthorizedResult();
        }
    }
}
