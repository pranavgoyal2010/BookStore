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
    public Task<string> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
    {
        return _userRL.ForgetPassword(forgetPasswordDto);
    }
    public Task<bool> ResetPassword(ResetPasswordWithOTPDto resetPasswordWithOTPDto)
    {
        return _userRL.ResetPassword(resetPasswordWithOTPDto);
    }
}
