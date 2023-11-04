namespace Agendo.AuthAPI.Model
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string SuperiorID { get; set; } = string.Empty;
    }
}
