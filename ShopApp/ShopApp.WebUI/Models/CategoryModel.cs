using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage ="Name cannot be empty")]
        [StringLength(60, MinimumLength = 5, ErrorMessage = "Name have to be between 5 and 60 characters!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Url cannot be empty")]
        [StringLength(60, MinimumLength = 5, ErrorMessage = "Url have to be between 5 and 60 characters!")]
        public string Url { get; set; }
        public List<Product> Products { get; set; }
    }
}
