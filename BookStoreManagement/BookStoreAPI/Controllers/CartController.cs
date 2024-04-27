using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Dto;
using ModelLayer.Response;
using RepositoryLayer.Entity;
using System.Security.Claims;

namespace BookStoreAPI.Controllers;

[Route("api/cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartBL _cartBL;

    public CartController(ICartBL cartBL)
    {
        _cartBL = cartBL;
    }

    [HttpGet]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> GetAllCartItems()
    {
        try
        {
            var userIdClaimed = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = Convert.ToInt32(userIdClaimed);

            var result = await _cartBL.GetAllCartItems(userId);

            var response = new ResponseModel<IEnumerable<CartItemEntity>>
            {
                Message = "All items retrieved from cart",
                Data = result
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            };
            return BadRequest(response);
        }
    }


    [HttpPost]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> AddToCart(CartItemDto cartItemDto)
    {
        try
        {
            var userIdClaimed = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = Convert.ToInt32(userIdClaimed);

            var result = await _cartBL.AddToCart(userId, cartItemDto);

            var response = new ResponseModel<CartItemEntity>
            {
                Message = "Item added to cart",
                Data = result
            };

            return Ok(response);

        }
        catch (Exception ex)
        {
            var response = new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            };
            return BadRequest(response);
        }
    }

    [HttpDelete("{cartItemId}")]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> RemoveFromCart(int cartItemId)
    {
        try
        {
            var result = await _cartBL.RemoveFromCart(cartItemId);

            var response = new ResponseModel<bool>
            {
                Message = "Item deleted from cart",
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            };
            return BadRequest(response);
        }
    }

    [HttpPatch("{cartItemId}")]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> UpdateToCart(int cartItemId, int quantity)
    {
        if (quantity <= 0)
        {
            return BadRequest("Quantity must be greater than zero.");
        }
        try
        {
            var result = await _cartBL.UpdateCart(cartItemId, quantity);
            var response = new ResponseModel<CartItemEntity>
            {
                Message = "Item updated in the cart",
                Data = result
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            };
            return BadRequest(response);
        }
    }
}
