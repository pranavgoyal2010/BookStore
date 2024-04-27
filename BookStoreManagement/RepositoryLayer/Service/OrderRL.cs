using Dapper;
using ModelLayer.Dto;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service;

public class OrderRL : IOrderRL
{
    private readonly BookStoreContext _bookStoreContext;

    public OrderRL(BookStoreContext bookStoreContext)
    {
        _bookStoreContext = bookStoreContext;
    }

    public async Task<bool> PlaceOrder(int userId, OrderDto orderDto)
    {
        var connection = _bookStoreContext.CreateConnection();

        try
        {
            connection.Open(); // Open the connection before using it

            using (var transaction = connection.BeginTransaction())
            {
                // Step 1: Insert new Order record
                var orderInsertQuery = "INSERT INTO [Order] (UserId, OrderDate, Address) VALUES (@UserId, @OrderDate, @Address); SELECT SCOPE_IDENTITY();";


                //have to write conditional code if user exist already then have to just update the row and if not then insert new row
                var orderId = await connection.ExecuteScalarAsync<int>(
                    orderInsertQuery,
                    new { UserId = userId, OrderDate = DateTime.Now, Address = orderDto.Address },
                    transaction
                );



                // Step 2: Retrieve all items from the user's cart
                var cartItemsQuery = @"
                        SELECT * FROM CartItem 
                        WHERE CartId = (SELECT CartId FROM Cart WHERE UserId = @UserId)";

                var cartItems = (await connection.QueryAsync<CartItemEntity>(cartItemsQuery, new { UserId = userId }, transaction)).ToList();
                // Check if the cart is empty
                if (!cartItems.Any()) //!false
                {
                    throw new InvalidOperationException("The cart is empty.");
                }


                // Step 3: Bulk insert cart items into OrderItem table
                var orderItemsInsertQuery = $@"
                         INSERT INTO OrderItem (OrderId, BookId, Quantity, Price)
                         VALUES ({orderId}, @BookId, @Quantity, @Price)";

                if (cartItems.Any()) //true
                {
                    await connection.ExecuteAsync(orderItemsInsertQuery, cartItems, transaction);
                }

                // Step 4: Calculate total amount
                decimal totalAmount = cartItems.Sum(item => item.Price * item.Quantity);

                // Step 5: Update Order with total amount
                var updateOrderAmountQuery = "UPDATE [Order] SET Amount = @Amount WHERE OrderId = @OrderId";
                await connection.ExecuteAsync(updateOrderAmountQuery, new { Amount = totalAmount, OrderId = orderId }, transaction);

                // Step 6: Clear the cart
                var clearCartQuery = "DELETE FROM CartItem WHERE CartId = (SELECT CartId FROM Cart WHERE UserId = @UserId)";
                await connection.ExecuteAsync(clearCartQuery, new { UserId = userId }, transaction);

                // Commit transaction if all steps are successful
                transaction.Commit();
                return true;
            }
        }
        catch (Exception ex)
        {
            // Log the exception details here or handle them as needed
            throw new Exception("An error occurred while placing the order: " + ex.Message, ex);
        }
        finally
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close(); // Ensure the connection is closed after operation
        }
    }
}


