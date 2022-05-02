using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ulx.Models
{
    public class Ad
    {
        [Key]
        public int Id { get; set; }
        
        [Required]  
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        public int CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public List<Picture> Pictures { get; set; }
        public string UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }

        public DateTime Created { get; set; }
        [NotMapped]
        public string CategoryName
        {
            get => (Category==null? "": Category.Name);
        }

    }
}
