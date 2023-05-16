using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebShop.DBConfiguration;
using WebShop.DTO;
using WebShop.EmailService;
using WebShop.Interfaces;
using WebShop.Models;

namespace WebShop.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly WebShopDbContext _webShopDbContext;
        private readonly IConfigurationSection _secretKey;
        private readonly IEmailService _emailService;

        public UserService(IMapper mapper, WebShopDbContext webShopDbContext, IConfiguration configuration, IEmailService emailService) 
        {
            _mapper = mapper;
            _webShopDbContext = webShopDbContext;
            _secretKey = configuration.GetSection("SecretKey");
            _emailService = emailService;
        }

        public UserDTO ActivateUser(long id)
        {
            var user = _webShopDbContext.Users.Find(id);

            if (user == null) { return null; }

            user.Approved = true;
            _webShopDbContext.SaveChanges();

            // posalji email
            var emailAddress = user.Email;
            var message = new Message(new string[] { $"{emailAddress}" }, "Profile activation", "Your profile is now active! WebShop App.");
            _emailService.SendEmail(message);

            return _mapper.Map<UserDTO>(user);
        }

        public List<UserDTO> GetAllUnactivatedUsers()
        {
            return _mapper.Map<List<UserDTO>>(_webShopDbContext.Users.Where(u => u.Approved == false));
        }

        public List<UserDTO> GetAllUsers()
        {
            return _mapper.Map<List<UserDTO>>(_webShopDbContext.Users);
        }

        public UserRegisterDTO GetUser(long id)
        {
            var user = _webShopDbContext.Users.Find(id);
            if(user == null) { return null; }
            return _mapper.Map<UserRegisterDTO>(user);
        }

        public TokenDTO Login(UserLoginDTO user)
        {
            if(user == null) { return null; }

            User user1 = _webShopDbContext.Users.FirstOrDefault(u => u.Email == user.Email);

            if(user1 == null) { return null; }

            if (BCrypt.Net.BCrypt.Verify(user.Password, user1.Password))
            {
                List<Claim> userClaims = new List<Claim>();

                if (user1.type.ToString() == "Admin")
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }
                if (user1.type.ToString() == "Seller")
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, "Seller"));
                }
                if (user1.type.ToString() == "Buyer")
                {
                    userClaims.Add(new Claim(ClaimTypes.Role, "Buyer"));
                }

                SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey.Value));
                var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:44304",
                    claims: userClaims,
                    expires: DateTime.Now.AddMinutes(20),
                    signingCredentials: signinCredentials
                    );
                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return new TokenDTO()
                {
                    Token = tokenString
                };
            }
            else return null;
        }

        public UserDTO Register(UserRegisterDTO user)
        {
            User user1 = _webShopDbContext.Users.FirstOrDefault(u => u.Email == user.Email);
            if(user1 != null) { return null; }
            
            User newUser = _mapper.Map<User>(user);
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);

            if (newUser.type.ToString() == "Seller")
            {
                newUser.Approved = false;
                newUser.Articles = new List<Article>();
            }
            else
            {
                newUser.Approved = true;
                newUser.Orders = new List<Order>();
            }
            _webShopDbContext.Users.Add(newUser);
            _webShopDbContext.SaveChanges();

            return _mapper.Map<UserDTO>(newUser);
        }

        public UserRegisterDTO UpdateUser(long id, UserRegisterDTO user)
        {
            var u = _webShopDbContext.Users.Find(id);
            if(u == null) { return null; }
            else
            {
                u.FullName = user.FullName;
                User user1 = _webShopDbContext.Users.FirstOrDefault(u1 => u1.Email == user.Email);
                if (user1 != null) { return null; }
                u.Email = user.Email; 
                u.Address = user.Address;
                u.Username = user.Username;
                u.BirthDate = user.BirthDate;
                u.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                u.ProfilePictureUrl = user.ProfilePictureUrl;

                _webShopDbContext.SaveChanges();
                return _mapper.Map<UserRegisterDTO>(u);
            }
        }
    }
}
