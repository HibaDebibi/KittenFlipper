using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KittenFlipper.Models.User
{
    public class LoginModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>admin</example>
        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>securePassword</example>
        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }

    }
}
