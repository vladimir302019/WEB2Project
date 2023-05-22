using AutoMapper;
using WebShop.DTO.OrderDTOs;
using WebShop.Interfaces;
using WebShop.Models;
using WebShop.Models.Enums;
using WebShop.Repositories.IRepositories;

namespace WebShop.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IMapper mapper, IUnitOfWork unit)
        {
            _mapper = mapper;
            _unitOfWork = unit;
        }

        public async Task<bool> CancelOrder(long orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetById(orderId);
            if (order == null) { return false; }

            List<Article> list = await _unitOfWork.ArticleRepository.GetAll();

            foreach (var item in order.OrderItems)
            {
                foreach (var article in list)
                {
                    if (item.ArticleId == article.Id)
                    {
                        article.MaxQuantity = article.MaxQuantity + item.Quantity;
                        _unitOfWork.ArticleRepository.UpdateArticle(article);
                    }
                }
            }

            order.Confirmed = false;
            _unitOfWork.OrderRepository.UpdateOrder(order);

            await _unitOfWork.Save();

            return true;
        }

        public async Task<OrderAllDTO> GetOrder(long orderId)
        {
            return _mapper.Map<OrderAllDTO>(await _unitOfWork.OrderRepository.GetById(orderId));
        }

        public async Task<List<OrderAdminDTO>> GetOrders()
        {
            return _mapper.Map<List<OrderAdminDTO>>(await _unitOfWork.OrderRepository.GetAll());
        }

        public async Task<List<OrderAllDTO>> GetUserOrders(long userId)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId);

            if (user == null) { return null; }

            return _mapper.Map<List<OrderAllDTO>>(user.Orders.Where(order => order.DeliveryDate < DateTime.Now && order.Confirmed));
        }

        public async Task<List<OrderAllDTO>> GetNewOrders(long userId)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId);

            if (user == null) { return null; }

            return _mapper.Map<List<OrderAllDTO>>(await _unitOfWork.OrderRepository.GetSellerOrders(userId, false));
        }

        public async Task<List<OrderAllDTO>> GetUndeliveredOrders(long userId)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId);

            if (user == null) { return null; }

            return _mapper.Map<List<OrderAllDTO>>(user.Orders.Where(order => order.DeliveryDate > DateTime.Now && order.Confirmed));
        }

        public async Task<OrderDTO> NewOrder(OrderDTO orderDTO, long buyerId)
        {
            var user = await _unitOfWork.UserRepository.GetById(buyerId);
            if (user == null) { return null; }

            Order newOrder = _mapper.Map<Order>(orderDTO);

            newOrder.Confirmed = true;
            newOrder.Buyer = user;
            newOrder.BuyerId = user.Id;
            Random random = new Random();
            int minutes = random.Next(65, 180);
            newOrder.DeliveryDate = DateTime.Now.AddMinutes(minutes);

            List<Article> list = await _unitOfWork.ArticleRepository.GetAll();
            List<User> allSellers = await _unitOfWork.UserRepository.GetAll();
            allSellers = allSellers.Where(s => s.type == UserType.Seller).ToList();
            List<long> ids = new List<long>();
            List<User> sellers = new List<User>();
            foreach (var item in newOrder.OrderItems)
            {
                newOrder.TotalPrice += item.Article.Price;
                ids.Add(item.Article.SellerId);
                foreach (var article in list)
                {
                    if (item.ArticleId == article.Id)
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
            foreach(var seller in allSellers)
            {
                if (ids.Contains(seller.Id))
                {
                    if (!sellers.Contains(seller))
                    {
                        sellers.Add(seller);
                    }
                }
            }
            newOrder.TotalPrice += 230 * sellers.Count();
            user.Orders.Add(newOrder);
            await _unitOfWork.OrderRepository.InsertOrder(newOrder);
            await _unitOfWork.Save();

            return _mapper.Map<OrderDTO>(newOrder);
        }

        public async Task<List<OrderAllDTO>> GetOldOrders(long userId)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId);

            if (user == null) { return null; }

            return _mapper.Map<List<OrderAllDTO>>(await _unitOfWork.OrderRepository.GetSellerOrders(userId, true));
        }
    }
}
