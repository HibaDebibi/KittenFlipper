using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using KittenFlipper.Contracts;
using KittenFlipper.Infrastructure.Jwt;
using KittenFlipper.Models.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace KittenFlipper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IUserService _userService;
        private readonly TokenManagement _tokenManagement;
        public LoginController(ILogger<LoginController> logger, IUserService userService, TokenManagement tokenManagement)
        {
            _logger = logger;
            _userService = userService;
            _tokenManagement = tokenManagement;
        }

        /// <summary>
        /// JWT login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Request");
            }

            var user = _userService.Authenticate(request.UserName, request.Password);

            if (user == null || user.Id == 0)
                return BadRequest(new { message = "Username or password is incorrect" });

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                _tokenManagement.Issuer,
                _tokenManagement.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            _logger.LogInformation($"User [{request.UserName}] logged in the system.");
            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token
            });
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public void Logout()
        {
           // HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}