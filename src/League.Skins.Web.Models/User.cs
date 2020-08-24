using System.ComponentModel.DataAnnotations;

namespace League.Skins.Web.Models
{
    public class UserLoginRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class UserRegisterRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class UserRelateRequest
    {
        [Required]
        public string Login { get; set; }
    }

    public class UserResponse
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string[] Editors { get; set; }
        public string[] Editable { get; set; }
    }
}
