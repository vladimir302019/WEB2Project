using WebShop.DTO;
using WebShop.Models;

namespace WebShop.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDTO> NewOrder(OrderDTO orderDTO, long buyerId);
        Task<OrderAllDTO> GetOrder(long orderId);
        Task<List<OrderAdminDTO>> GetOrders();
        Task<List<OrderAllDTO>> GetUndeliveredOrders(long userId);
        Task<List<OrderAllDTO>> GetNewOrders(long userId);
        Task<List<OrderAllDTO>> GetOldOrders(long userId);
        Task<List<OrderAllDTO>> GetUserOrders(long userId);
        Task<List<OrderItemDTO>> GetOrderedItems(long sellerId);
        Task<bool> CancelOrder(long orderId);

    }
}
