namespace KittenFlipper.Models.Login
{
    public class LoginResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>admin</example>
        public string UserName { get; set; }
        public string JwtToken { get; set; }
    }
}
