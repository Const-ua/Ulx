using System.ComponentModel.DataAnnotations;

namespace Ulx.Models
{
    public class RegisterVM
    {
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter correct Email")]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password, ErrorMessage = "Password don't match!")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Phone { get; set; }
    }

}
