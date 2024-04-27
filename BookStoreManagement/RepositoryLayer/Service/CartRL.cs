using Dapper;
using ModelLayer.Dto;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service;

public class CartRL : ICartRL
{
    private readonly BookStoreContext _bookStoreContext;

    public CartRL(BookStoreContext bookStoreContext)
    {
        _bookStoreContext = bookStoreContext;
    }

    public async Task<IEnumerable<CartItemEntity>> GetAllCartItems(int userId)
    {

        var cartIdQuery = "SELECT CartId FROM Cart WHERE UserId = @UserId";

        var selectQuery = "SELECT * FROM CartItem WHERE CartId = @CartId";

        using (var connect = _bookStoreContext.CreateConnection())
        {
            var cartId = await connect.QueryFirstOrDefaultAsync<int>(cartIdQuery, new { UserId = userId });

            IEnumerable<CartItemEntity> cartItems = await connect.QueryAsync<CartItemEntity>(selectQuery, new { CartId = cartId });

            return cartItems.ToList();
        }
    }

    public async Task<CartItemEntity> AddToCart(int userId, CartItemDto cartItemDto)
    {
        //var res = 0;

        var insertQuery = "INSERT INTO CartItem (CartId, BookId, Quantity, Price) VALUES (@CartId, @BookId, @Quantity, @Price)";

        var cartIdQuery = "SELECT CartId FROM Cart WHERE UserId = @UserId"; //Query Getting cart id for specifc user

        using (var connect = _bookStoreContext.CreateConnection())
        {
            var cartId = await connect.QueryFirstOrDefaultAsync<int>(cartIdQuery, new { UserId = userId }); //Cart Id of user who got an empty cart when user registered

            if (cartId == default) // Check for default value (usually 0 for int)
            {
                // Create a new cart if it doesn't exist
                cartId = await CreateCart(userId);
            }


            var price = await connect.QueryFirstOrDefaultAsync<decimal>("SELECT Price FROM Books WHERE BookId=@bookId", new { bookId = cartItemDto.BookId });

            var res = await connect.ExecuteAsync(insertQuery, new
            {
                CartId = cartId,
                BookId = cartItemDto.BookId,
                Quantity = cartItemDto.Quantity,
                Price = price
            });
            if (res == 0)
            {
                throw new Exception("Couldn't Add to Cart!");
            }

            var CurrentCartData = await connect.QueryFirstOrDefaultAsync<CartItemEntity>("SELECT TOP 1 * FROM CartItem ORDER BY CartItemId DESC");

            return CurrentCartData;

        }
    }

    public async Task<bool> RemoveFromCart(int cartItemId)
    {
        //var res = 0;
        var query = "DELETE FROM CartItem WHERE CartItemId=@CartItemId";

        using (var connect = _bookStoreContext.CreateConnection())
        {
            var res = await connect.ExecuteAsync(query, new { CartItemId = cartItemId });
            if (res == 0)
            {
                throw new Exception("Couldn't Remove from Cart!");
            }
            return true;
        }
    }

    public async Task<CartItemEntity> UpdateCart(int cartItemId, int quantity)
    {

        //var res = 0;
        var query = "UPDATE CartItem SET Quantity=@Quantity WHERE CartItemId=@CartItemId";

        using (var connect = _bookStoreContext.CreateConnection())
        {
            var res = await connect.ExecuteAsync(query, new { Quantity = quantity, CartItemId = cartItemId });

            if (res == 0)
            {
                throw new Exception("Couldn't Update Cart!");
            }

            var CurrentCartUpdateData = await connect.QueryFirstOrDefaultAsync<CartItemEntity>("SELECT * FROM CartItem WHERE CartItemId = @CartItemId", new { CartItemId = cartItemId });


            return CurrentCartUpdateData;

        }
    }

    private async Task<int> CreateCart(int userId)
    {
        string createCartQuery = @" INSERT INTO Cart (UserId) OUTPUT INSERTED.CartId VALUES (@UserId);";

        using (var connection = _bookStoreContext.CreateConnection())
        {
            var cartId = await connection.QuerySingleAsync<int>(createCartQuery, new { UserId = userId });
            return cartId;
        }
    }
}
