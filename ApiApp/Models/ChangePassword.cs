using System.ComponentModel.DataAnnotations;

namespace ApiApp.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Enter User Name")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Enter Old Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Enter New Password")]
        public string NewPassword { get; set; }
    }
}
