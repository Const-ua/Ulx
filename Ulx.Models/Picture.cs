using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ulx.Models
{
    public class Picture
    {
        [Key]
        public int  Id { get; set; }
        [Required]
        public int AdId { get; set; }

        [ForeignKey("AdId")]
        public Ad Ad { get; set; }
        public byte[] Photo { get; set; }

    }
}
