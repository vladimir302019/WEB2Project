using AutoMapper;
using WebShop.DBConfiguration;
using WebShop.DTO;
using WebShop.Interfaces;
using WebShop.Models;
using WebShop.Models.Enums;
using WebShop.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<OrderItemDTO>> GetOrderedItems(long sellerId)
        {
            var user = await _unitOfWork.UserRepository.GetById(sellerId);
            if(user == null) { return null; }

            List<OrderItemDTO> sellerOrderItems = new List<OrderItemDTO>();

            List<OrderAllDTO> orderAllDTOs = new List<OrderAllDTO>();
                
            orderAllDTOs = _mapper.Map<List<OrderAllDTO>>(await _unitOfWork.OrderRepository.GetAll());
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

        public async Task<List<OrderAdminDTO>> GetOrders()
        {
            return _mapper.Map<List<OrderAdminDTO>>(await _unitOfWork.OrderRepository.GetAll());
        }

        public async Task<List<OrderAllDTO>> GetUserOrders(long userId)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId);

            if(user == null) { return null; }

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
            if(user == null){ return null; }

            Order newOrder = _mapper.Map<Order>(orderDTO);

            newOrder.Confirmed = true;
            newOrder.Buyer = user;
            newOrder.BuyerId = user.Id;
            Random random = new Random();
            int minutes = random.Next(65, 180);
            newOrder.DeliveryDate = DateTime.Now.AddMinutes(minutes);

            List<Article> list = await _unitOfWork.ArticleRepository.GetAll();

            foreach(var item in newOrder.OrderItems)
            {
                foreach(var article in list)
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
