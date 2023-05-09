namespace WebShop.DTO
{
    public class TokenDTO
    {
        public string Token { get; set; }   
        public string Role { get; set; }
        public long UserId { get; set; }
        public bool Active { get; set; }
    }
}
