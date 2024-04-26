using ModelLayer.Dto;

namespace BusinessLayer.Interface;

public interface IUserBL
{
    public Task<bool> Register(UserRegistrationDto userRegistrationDto);
    public Task<string> Login(UserLoginDto userLoginDto);
}
