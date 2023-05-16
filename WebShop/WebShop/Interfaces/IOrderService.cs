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
        List<OrderAllDTO> GetUnapprovedOrders(long userId);
        List<OrderAllDTO> GetOrders(long userId);
        List<OrderItemDTO> GetOrderedItems(long sellerId);
        bool ConfirmOrder(long orderId);
        bool CancelOrder(long orderId);

    }
}
