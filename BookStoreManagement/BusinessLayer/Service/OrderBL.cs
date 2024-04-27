using BusinessLayer.Interface;
using ModelLayer.Dto;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service;

public class OrderBL : IOrderBL
{
    private readonly IOrderRL _orderRL;

    public OrderBL(IOrderRL orderRL)
    {
        _orderRL = orderRL;
    }
    public Task<bool> PlaceOrder(int userId, OrderDto orderDto)
    {
        return _orderRL.PlaceOrder(userId, orderDto);
    }
}
