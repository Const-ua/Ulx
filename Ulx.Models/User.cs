using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Ulx.Models
{
    public class User : IdentityUser

    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [Phone]
        public string? Phone { get; set; }
    }
}
