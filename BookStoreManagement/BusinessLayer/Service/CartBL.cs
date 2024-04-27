using BusinessLayer.Interface;
using ModelLayer.Dto;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service;

public class CartBL : ICartBL
{
    private readonly ICartRL _cartRL;

    public CartBL(ICartRL cartRL)
    {
        _cartRL = cartRL;
    }

    public Task<IEnumerable<CartItemEntity>> GetAllCartItems(int userId)
    {
        return _cartRL.GetAllCartItems(userId);
    }

    public Task<CartItemEntity> AddToCart(int userId, CartItemDto cartItemDto)
    {
        return _cartRL.AddToCart(userId, cartItemDto);
    }

    public Task<bool> RemoveFromCart(int cartItemId)
    {
        return _cartRL.RemoveFromCart(cartItemId);
    }

    public Task<CartItemEntity> UpdateCart(int cartItemId, int quantity)
    {
        return _cartRL.UpdateCart(cartItemId, quantity);
    }
}
