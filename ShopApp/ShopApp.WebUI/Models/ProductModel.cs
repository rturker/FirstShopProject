using ShopApp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        //[Required(ErrorMessage = "Name cannot be empty!")]
        //[StringLength(60, MinimumLength = 5, ErrorMessage = "Name have to be between 5 and 60 characters!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price cannot be empty!")]
        [Range(0, 2000000, ErrorMessage ="Price have to be between 0 and 2m")]
        public double? Price { get; set; }

        [Required(ErrorMessage = "Description cannot be empty!")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Description have to be between 5 and 200 characters!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ImageUrl cannot be empty!")]
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }

        [Required(ErrorMessage = "Url cannot be empty!")]
        public string Url { get; set; }
        public List<Category> SelectedCategories { get; set; }
    }
}
