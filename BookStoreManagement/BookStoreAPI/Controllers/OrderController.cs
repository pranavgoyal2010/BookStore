using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Dto;
using ModelLayer.Response;
using System.Security.Claims;

namespace BookStoreAPI.Controllers;

[Route("api/order")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderBL _orderBL;

    public OrderController(IOrderBL orderBL)
    {
        _orderBL = orderBL;
    }

    [HttpPost]
    [Authorize(Roles = "customer")]
    public async Task<IActionResult> PlaceOrder(OrderDto orderDto)
    {
        try
        {
            var userIdClaimed = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId = Convert.ToInt32(userIdClaimed);

            var result = await _orderBL.PlaceOrder(userId, orderDto);

            var response = new ResponseModel<bool>
            {
                Message = "Order placed",
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
