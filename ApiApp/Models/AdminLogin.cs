using System.ComponentModel.DataAnnotations;

namespace ApiApp.Models
{
    public class AdminLogin
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }

}
