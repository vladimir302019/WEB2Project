using Microsoft.EntityFrameworkCore;
using WebShop.DBConfiguration;
using WebShop.Models;
using WebShop.Repositories.IRepositories;

namespace WebShop.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly WebShopDbContext _context;
        public OrderRepository(WebShopDbContext webShopDbContext) { _context = webShopDbContext; }
        public void DeleteOrder(long id)
        {
            _context.Orders.Remove(_context.Orders.Find(id));
        }

        public async Task<List<Order>> GetAll()
        {
            List<Order> orders = await _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Article).ToListAsync();
            return orders;
        }

        public async Task<Order> GetById(long id)
        {
            var order = await _context.Orders.Where(o => o.Id == id)
                                .Include(oi => oi.OrderItems)
                                .ThenInclude(a => a.Article).FirstOrDefaultAsync();
            return order;
        }

        public async Task<List<Order>> GetSellerOrders(long sellerId, bool old)
        {
            List<Order> orders = await _context.Orders.Where(o => o.Confirmed && 
                                (old ? o.DeliveryDate < DateTime.Now : o.DeliveryDate > DateTime.Now))
                                .Include(x => x.OrderItems.Where(oi => oi.Article.SellerId == sellerId))
                                .ThenInclude(x => x.Article)
                                .ToListAsync();

            return orders.FindAll(o => o.OrderItems.FindAll(oi => oi.Article.SellerId == sellerId).Count != 0);
        }

        public async Task<Order> InsertOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            return order;
        }

        public void UpdateOrder(Order order)
        {
            Order newOrder = _context.Orders.Find(order.Id);
            newOrder = order;
            _context.Orders.Update(newOrder);
        }
    }
}
