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
            throw new NotImplementedException();
        }

        public bool ConfirmOrder(long orderId)
        {
            throw new NotImplementedException();
        }

        public OrderAllDTO GetOrder(long orderId)
        {
            return _mapper.Map<OrderAllDTO>(_dbContext.Orders.Find(orderId));
        }

        public List<OrderItemDTO> GetOrderedItems(long sellerId)
        {
            List<OrderItemDTO> sellerOrderItems = new List<OrderItemDTO>();

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
        }

        public List<OrderAdminDTO> GetOrders()
        {
            return _mapper.Map<List<OrderAdminDTO>>(_dbContext.Orders);
        }

        public List<OrderAllDTO> GetOrders(long userId)
        {
            var user = _dbContext.Users.Find(userId);

            if(user == null) { return null; }

            return _mapper.Map<List<OrderAllDTO>>(_dbContext.Orders.Where(order => order.BuyerId == userId && order.DeliveryDate < DateTime.Now && order.Confirmed));
        }

        public List<OrderAllDTO> GetUnapprovedOrders(long userId)
        {
            var user = _dbContext.Users.Find(userId);

            if (user == null) { return null; }

            List<OrderItemDTO> orderedItemDTOs = GetOrderedItems(userId);
            List<OrderAllDTO> ordersAllDTOs = _mapper.Map<List<OrderAllDTO>>(_dbContext.Orders.Where(order => !order.Approved)); 
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

            return _mapper.Map<List<OrderAllDTO>>(_dbContext.Orders.Where(order => order.BuyerId == userId && order.DeliveryDate > DateTime.Now && !order.Confirmed));

        }

        public OrderDTO NewOrder(OrderDTO orderDTO, long buyerId)
        {
            throw new NotImplementedException();
        }
    }
}
