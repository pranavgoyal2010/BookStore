using BusinessLayer.Interface;
using ModelLayer.Dto;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service;

public class UserBL : IUserBL
{
    private readonly IUserRL _userRL;

    public UserBL(IUserRL userRL)
    {
        _userRL = userRL;
    }
    public Task<bool> Register(UserRegistrationDto userRegistrationDto)
    {
        return _userRL.Register(userRegistrationDto);
    }
    public Task<string> Login(UserLoginDto userLoginDto)
    {
        return _userRL.Login(userLoginDto);
    }
}
