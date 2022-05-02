using System.ComponentModel.DataAnnotations;

namespace Ulx.Models
{
    public class LoginVM
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Incorrect email address")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password, ErrorMessage = "Enter your password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
