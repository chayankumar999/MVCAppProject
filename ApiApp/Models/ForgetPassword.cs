using System.ComponentModel.DataAnnotations;

namespace ApiApp.Models
{
    public class ForgetPassword
    {

        [EmailAddress]
        [Required(ErrorMessage = "Enter Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Enter User Name")]
        public string Username { get; set; }

        [Phone]
        [Required(ErrorMessage = "Enter Your Phone Number")]
        public string Phone { get; set; }
    }
}
