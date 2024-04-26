using BusinessLayer.Interface;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.CustomException;
using ModelLayer.Dto;
using ModelLayer.Response;

namespace BookStoreAPI.Controllers;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserBL _userBL;

    public UserController(IUserBL userBL)
    {
        _userBL = userBL;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegistrationDto userRegistrationDto)
    {
        try
        {
            bool registrationResult = await _userBL.Register(userRegistrationDto);
            if (registrationResult)
            {
                var response = new ResponseModel<string>
                {
                    Success = true,
                    Message = "User Registration Successful"
                };
                return Ok(response);
            }

            return BadRequest("Invalid input");
        }
        catch (EmailAlreadyExistsException ex)
        {

            var errorResponse = new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            };
            return Conflict(errorResponse);
        }
        catch (Exception ex)
        {
            var errorResponse = new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            };
            return StatusCode(500, errorResponse);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        try
        {
            string token = await _userBL.Login(userLoginDto);

            var response = new ResponseModel<string>
            {
                Message = "Login Successful",
                Data = token
            };
            return Ok(response);
        }
        catch (InvalidLoginException ex)
        {
            var errorResponse = new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            };
            return BadRequest(errorResponse);
        }
        catch (Exception ex)
        {
            var errorResponse = new ResponseModel<string>
            {
                Success = false,
                Message = ex.Message
            };
            return StatusCode(500, errorResponse);
        }
    }
}
