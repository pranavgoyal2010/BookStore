using ModelLayer.Dto;

namespace RepositoryLayer.Interface;

public interface IUserRL
{
    public Task<bool> Register(UserRegistrationDto userRegistrationDto);
    public Task<string> Login(UserLoginDto userLoginDto);
    public Task<string> ForgetPassword(ForgetPasswordDto forgetPasswordDto);
    public Task<bool> ResetPassword(ResetPasswordWithOTPDto resetPasswordWithOTPDto);
}

