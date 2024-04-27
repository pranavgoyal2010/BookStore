using ModelLayer.Dto;

namespace RepositoryLayer.Interface;

public interface IOrderRL
{
    public Task<bool> PlaceOrder(int userId, OrderDto orderDto);
}
