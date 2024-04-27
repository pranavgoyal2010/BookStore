using ModelLayer.Dto;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface;

public interface ICartRL
{
    public Task<IEnumerable<CartItemEntity>> GetAllCartItems(int userId);
    public Task<CartItemEntity> AddToCart(int userId, CartItemDto cartItemDto);
    public Task<bool> RemoveFromCart(int cartItemId);
    public Task<CartItemEntity> UpdateCart(int cartItemId, int quantity);
}
