using System.ComponentModel.DataAnnotations;

namespace CoffeeShop.Models
{
    public class CoffeeProduct
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Range(0, 10000)]
        public decimal Price { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;
        
        public string ImageUrl { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string Origin { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string RoastLevel { get; set; } = string.Empty;
        
        [Range(0, 1000)]
        public int Stock { get; set; }
    }
}