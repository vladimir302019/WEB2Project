using WebShop.Models.Enums;

namespace WebShop.Models
{
    public class User
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public string ProfilePictureUrl { get; set;}
        public string Address { get; set; }
        public UserType type { get; set; }
        public bool Approved { get; set; }
        public List<Order> Orders { get; set; }
        public List<Article> Articles { get; set; }
    }
}
