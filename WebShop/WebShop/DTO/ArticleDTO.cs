using System.ComponentModel.DataAnnotations;

namespace WebShop.DTO
{
    public class ArticleDTO
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int MaxQuantity { get; set; }
        [Required]
        public string PhotoUrl { get; set; }
    }
}
