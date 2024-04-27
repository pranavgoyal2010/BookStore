using ModelLayer.Dto;

namespace BusinessLayer.Interface;

public interface IOrderBL
{
    public Task<bool> PlaceOrder(int userId, OrderDto orderDto);
}
