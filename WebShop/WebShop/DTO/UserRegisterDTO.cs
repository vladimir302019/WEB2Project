using WebShop.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace WebShop.DTO
{
    public class UserRegisterDTO
    {
        public long Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string ProfilePictureUrl { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public UserType type { get; set; }
    }
}
