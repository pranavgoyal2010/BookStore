using ModelLayer.Dto;

namespace RepositoryLayer.Interface;

public interface IUserRL
{
    public Task<bool> Register(UserRegistrationDto userRegistrationDto);
    public Task<bool> Login(UserLoginDto userLoginDto);
    //public Task<bool> ResetPasswordAsync(string userEmail, string oldPassword, string newPassword);
    //public Task<string> ForgotPasswordAsync(string userEmail);
}

