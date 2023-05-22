using System.ComponentModel.DataAnnotations;
using WebShop.Models;

namespace WebShop.DTO.OrderDTOs
{
    public class OrderDTO
    {
        public long Id { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public double TotalPrice { get; set; }
        [Required]
        public List<OrderItem> OrderItems { get; set; }
    }
}
