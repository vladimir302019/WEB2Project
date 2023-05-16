using AutoMapper;
using WebShop.DBConfiguration;
using WebShop.DTO;
using WebShop.Interfaces;
using WebShop.Models;
using WebShop.Models.Enums;

namespace WebShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly WebShopDbContext _dbContext;

        public OrderService(IMapper mapper, WebShopDbContext webShopDbContext)
        {
            _mapper = mapper;
            _dbContext = webShopDbContext;
        }

        public bool CancelOrder(long orderId)
        {
            var order = _dbContext.Orders.Find(orderId);
            if (order == null) { return false; }

            order.Confirmed = false;
            _dbContext.SaveChanges();

            return true;
        }

        public OrderAllDTO GetOrder(long orderId)
        {
            return _mapper.Map<OrderAllDTO>(_dbContext.Orders.Find(orderId));
        }

        public List<OrderItemDTO> GetOrderedItems(long sellerId)
        {
            var user = _dbContext.Users.Find(sellerId);
            if(user == null) { return null; }
            return _mapper.Map<List<OrderItemDTO>>(user.Articles);

            /*List<OrderItemDTO> sellerOrderItems = new List<OrderItemDTO>();

            var user = _dbContext.Users.Find(sellerId);
            if (user == null) { return null; }

            List<OrderAllDTO> orderAllDTOs = new List<OrderAllDTO>();
                
            orderAllDTOs = _mapper.Map<List<OrderAllDTO>>(_dbContext.Orders);
            foreach (OrderAllDTO o in orderAllDTOs)
            {
                foreach (OrderItem oi in o.OrderItems)
                {
                    if (oi.Article.SellerId == sellerId)
                    {
                        sellerOrderItems.Add(_mapper.Map<OrderItemDTO>(oi));
                    }
                }
            }
            return sellerOrderItems;
                */
        }

        public List<OrderAdminDTO> GetOrders()
        {
            return _mapper.Map<List<OrderAdminDTO>>(_dbContext.Orders);
        }

        public List<OrderAllDTO> GetOrders(long userId)
        {
            var user = _dbContext.Users.Find(userId);

            if(user == null) { return null; }

            return _mapper.Map<List<OrderAllDTO>>(user.Orders.Where(order => order.DeliveryDate < DateTime.Now && order.Confirmed));
        }

        public List<OrderAllDTO> GetNewOrders(long userId)
        {
            var user = _dbContext.Users.Find(userId);

            if (user == null) { return null; }

            List<OrderItemDTO> orderedItemDTOs = GetOrderedItems(userId);
            List<OrderAllDTO> ordersAllDTOs = _mapper.Map<List<OrderAllDTO>>(_dbContext.Orders.Where(order => order.DeliveryDate > DateTime.Now && order.Confirmed)); 
            List<OrderAllDTO> sellerOrders = new List<OrderAllDTO>();

            foreach(var order in ordersAllDTOs)
            {
                foreach(var item in orderedItemDTOs)
                {
                    if(order.Id == item.OrderId)
                    {
                        if(!sellerOrders.Contains(order))
                        sellerOrders.Add(order);
                    }
                }
            }
            return sellerOrders;
        }

        public List<OrderAllDTO> GetUndeliveredOrders(long userId)
        {
            var user = _dbContext.Users.Find(userId);

            if (user == null) { return null; }

            return _mapper.Map<List<OrderAllDTO>>(user.Orders.Where(order => order.DeliveryDate > DateTime.Now && order.Confirmed));
        }

        public OrderDTO NewOrder(OrderDTO orderDTO, long buyerId)
        {
            var user = _dbContext.Users.Find(buyerId);
            if(user == null)
            {
                return null;
            }

            Order newOrder = _mapper.Map<Order>(orderDTO);

            newOrder.Confirmed = true;
            newOrder.Buyer = user;
            newOrder.BuyerId = user.Id;
            Random random = new Random();
            int minutes = random.Next(65, 180);
            newOrder.DeliveryDate = DateTime.Now.AddMinutes(minutes);

            foreach(var item in newOrder.OrderItems)
            {
                foreach(var article in _dbContext.Articles)
                {
                    if(item.ArticleId == article.Id)
                    {
                        if (article.MaxQuantity >= item.Quantity)
                        {
                            article.MaxQuantity = article.MaxQuantity - item.Quantity;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }

            user.Orders.Add(newOrder);
            _dbContext.Orders.Add(newOrder);
            _dbContext.SaveChanges();

            return _mapper.Map<OrderDTO>(newOrder);
        }

        public List<OrderAllDTO> GetOldOrders(long userId)
        {
            var user = _dbContext.Users.Find(userId);

            if (user == null) { return null; }

            List<OrderItemDTO> orderedItemDTOs = GetOrderedItems(userId);
            List<OrderAllDTO> ordersAllDTOs = _mapper.Map<List<OrderAllDTO>>(_dbContext.Orders.Where(order => order.DeliveryDate < DateTime.Now && order.Confirmed));
            List<OrderAllDTO> sellerOrders = new List<OrderAllDTO>();

            foreach (var order in ordersAllDTOs)
            {
                foreach (var item in orderedItemDTOs)
                {
                    if (order.Id == item.OrderId)
                    {
                        if (!sellerOrders.Contains(order))
                            sellerOrders.Add(order);
                    }
                }
            }
            return sellerOrders;
        }
    }
}
