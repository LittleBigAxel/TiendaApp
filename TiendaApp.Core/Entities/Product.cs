using System.ComponentModel.DataAnnotations;

namespace TiendaApp.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000000, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock is required")]
        [Range(0, 1000000, ErrorMessage = "Stock must be between 0 and 1,000,000")]
        public int Stock { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL")]
        public string ImageURL { get; set; } = string.Empty;
    }
}