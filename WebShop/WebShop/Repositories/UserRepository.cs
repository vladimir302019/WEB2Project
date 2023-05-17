using Microsoft.EntityFrameworkCore;
using WebShop.DBConfiguration;
using WebShop.Models;
using WebShop.Repositories.IRepositories;

namespace WebShop.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly WebShopDbContext _context;
        public UserRepository(WebShopDbContext webShopDbContext) { _context = webShopDbContext; }
        public void DeleteUser(long id)
        {
            _context.Users.Remove(_context.Users.Find(id));
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> InsertUser(User user)
        {
            await _context.Users.AddAsync(user);
            return user;
        }

        public void UpdateUser(User user)
        {
            User newUser = _context.Users.Find(user.Id);
            newUser = user;
            _context.Users.Update(newUser);
        }
    }
}
