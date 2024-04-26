using ModelLayer.Dto;

namespace BusinessLayer.Interface;

public interface IUserBL
{
    public Task<bool> Register(UserRegistrationDto userRegistrationDto);
    public Task<bool> Login(UserLoginDto userLoginDto);
}
