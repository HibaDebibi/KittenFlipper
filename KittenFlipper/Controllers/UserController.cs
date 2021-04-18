using System;
using System.Collections.Generic;
using System.Linq;
using KittenFlipper.Helper.BasicAuth;
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

            public UserController(ILogger<UserController> logger)
            {
                _logger = logger;
            }

            /// <summary>
            /// API allows anonymous
            /// </summary>
            /// <returns></returns>
            [HttpGet]
            [AllowAnonymous]
            public IEnumerable<int> Get()
            {
                var rng = new Random();
                return Enumerable.Range(1, 3).Select(x => rng.Next(0, 100));
            }

            /// <summary>
            /// API requires JWT auth
            /// </summary>
            /// <returns></returns>
            [HttpGet("jwt")]
            [Authorize]
            public IEnumerable<int> JwtAuth()
            {
                var username = User.Identity.Name;
                _logger.LogInformation($"User [{username}] is visiting jwt auth with token {1}");
                var rng = new Random();
                return Enumerable.Range(1, 10).Select(x => rng.Next(0, 100));
            }

            /// <summary>
            /// API requires Basic auth
            /// </summary>
            /// <returns></returns>
            [HttpGet("basic")]
            [BasicAuth] // You can optionally provide a specific realm --> [BasicAuth("my-realm")]
            public IEnumerable<int> BasicAuth()
            {
                _logger.LogInformation("basic auth");
                var rng = new Random();
                return Enumerable.Range(1, 10).Select(x => rng.Next(0, 100));
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
