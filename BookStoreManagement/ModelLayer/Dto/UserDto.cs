namespace ModelLayer.Dto;

public class UserRegistrationDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MobileNo { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}

public class UserLoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class ForgetPasswordDto
{
    public string Email { get; set; }
}

public class ResetPasswordWithOTPDto
{
    public string Email { get; set; }
    public string OTP { get; set; }
    public string NewPassword { get; set; }
}
