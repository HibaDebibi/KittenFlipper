using KittenFlipper.Contracts;
using Microsoft.Extensions.Logging;

namespace KittenFlipper.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        // inject database for user validation
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public bool IsValidUser(string userName, string password)
        {
            _logger.LogInformation($"Validating user [{userName}]");
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            return true;
        }
    }
}
