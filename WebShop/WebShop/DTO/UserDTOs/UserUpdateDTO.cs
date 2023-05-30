using WebShop.Models.Enums;

namespace WebShop.DTO.UserDTOs
{
    public class UserUpdateDTO
    {
        
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public IFormFile ProfilePictureUrl { get; set; }
        public string Address { get; set; }
    }
}
