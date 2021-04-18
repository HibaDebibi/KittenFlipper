using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KittenFlipper.Models
{
    public class LoginRequest
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
