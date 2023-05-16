using WebShop.DTO;
using WebShop.Models;

namespace WebShop.Interfaces
{
    public interface IOrderService
    {
        OrderDTO NewOrder(OrderDTO orderDTO, long buyerId);
        OrderAllDTO GetOrder(long orderId);
        List<OrderAdminDTO> GetOrders();
        List<OrderAllDTO> GetUndeliveredOrders(long userId);
        List<OrderAllDTO> GetNewOrders(long userId);
        List<OrderAllDTO> GetOldOrders(long userId);
        List<OrderAllDTO> GetOrders(long userId);
        List<OrderItemDTO> GetOrderedItems(long sellerId);
        bool CancelOrder(long orderId);

    }
}
