using WebShop.Models.Enums;

namespace WebShop.DTO.UserDTOs
{
    public class UserUpdateDTO
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string Address { get; set; }
    }
}
