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
using WebShop.Repositories.IRepositories;

namespace WebShop.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfigurationSection _secretKey;
        private readonly IEmailService _emailService;

        public UserService(IMapper mapper, IUnitOfWork unit, IConfiguration configuration, IEmailService emailService) 
        {
            _mapper = mapper;
            _unitOfWork = unit;
            _secretKey = configuration.GetSection("SecretKey");
            _emailService = emailService;
        }

        public async Task<UserDTO> ActivateUser(long id, bool activate)
        {
            var user = await _unitOfWork.UserRepository.GetById(id);

            if (user == null) { return null; }
            if (activate)
            {
                user.Approved = true;

                var emailAddress = user.Email;
                var message = new Message(new string[] { $"{emailAddress}" }, "Profile activation", "Your profile is now active! WebShop App.");
                await _emailService.SendEmail(message);
            }
            else
            {
                var emailAddress = user.Email;
                var message = new Message(new string[] { $"{emailAddress}" }, "Profile activation", "Your profile activation has been rejected. WebShop App.");
                await _emailService.SendEmail(message);
            }

            _unitOfWork.UserRepository.UpdateUser(user);
            await _unitOfWork.Save();
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<List<UserDTO>> GetAllUnactivatedUsers()
        {
            List<User> list = await _unitOfWork.UserRepository.GetAll();
            return _mapper.Map<List<UserDTO>>(list.Where(u => u.Approved == false).ToList());
        }

        public async Task<List<UserDTO>> GetAllUsers()
        {
            return _mapper.Map<List<UserDTO>>(await _unitOfWork.UserRepository.GetAll());
        }

        public async Task<UserRegisterDTO> GetUser(long id)
        {
            var user = await _unitOfWork.UserRepository.GetById(id);
            if(user == null) { return null; }
            return _mapper.Map<UserRegisterDTO>(user);
        }

        public async Task<TokenDTO> Login(UserLoginDTO user)
        {
            if(user == null) { return null; }

            List<User> list = await _unitOfWork.UserRepository.GetAll();
            User user1 = list.Find(u => u.Email == user.Email);

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

        public async Task<UserDTO> Register(UserRegisterDTO user)
        {
            List<User> list = await _unitOfWork.UserRepository.GetAll();
            User user1 = list.Find(u => u.Email == user.Email);
            if (user1 != null) { return null; }
            
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
            await _unitOfWork.UserRepository.InsertUser(newUser);
            
            await _unitOfWork.Save();

            return _mapper.Map<UserDTO>(newUser);
        }

        public async Task<UserRegisterDTO> UpdateUser(long id, UserRegisterDTO user)
        {
            User u = await _unitOfWork.UserRepository.GetById(id);
            if(u == null) { return null; }
            else
            {
                List<User> list = await _unitOfWork.UserRepository.GetAll();
                User user1 = list.Find(u => u.Email == user.Email);
                if (user1 != null) { return null; }
                u = _mapper.Map<User>(user);
                u.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                _unitOfWork.UserRepository.UpdateUser(u);
                await _unitOfWork.Save();
                return _mapper.Map<UserRegisterDTO>(u);
            }
        }
    }
}
