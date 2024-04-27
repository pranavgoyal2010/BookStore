using Dapper;
using Microsoft.Extensions.Caching.Memory;
using ModelLayer.CustomException;
using ModelLayer.Dto;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System.Text.RegularExpressions;

namespace RepositoryLayer.Service;

public class UserRL : IUserRL
{
    private readonly BookStoreContext _bookStoreContext;
    private readonly IAuthServiceRL _authServiceRL;
    private readonly IMemoryCache _cache;
    private readonly IEmailServiceRL _emailServiceRL;

    public UserRL(BookStoreContext bookStoreContext, IAuthServiceRL authServiceRL, IMemoryCache cache, IEmailServiceRL emailServiceRL)
    {
        _bookStoreContext = bookStoreContext;
        _authServiceRL = authServiceRL;
        _cache = cache;
        _emailServiceRL = emailServiceRL;
    }

    public async Task<bool> Register(UserRegistrationDto userRegistrationDto)
    {
        if (!IsValidEmail(userRegistrationDto.Email))
            throw new InvalidEmailFormatException("Invalid email format");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegistrationDto.Password);

        var insertQuery = @"
        INSERT INTO [User] (FirstName, LastName, MobileNo, Email, Password, AddedOn, UpdatedOn, Role)
        VALUES (@FirstName, @LastName, @MobileNo, @Email, @Password, @AddedOn, @UpdatedOn, @Role);";

        using (var connection = _bookStoreContext.CreateConnection())
        {
            // Check if the email already exists
            var emailExists = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM [User] WHERE Email = @Email", new { Email = userRegistrationDto.Email });

            if (emailExists > 0)
                throw new EmailAlreadyExistsException("Email already exists");

            var user = new UserEntity
            {
                FirstName = userRegistrationDto.FirstName,
                LastName = userRegistrationDto.LastName,
                MobileNo = userRegistrationDto.MobileNo,
                Email = userRegistrationDto.Email,
                Password = hashedPassword,
                AddedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                Role = userRegistrationDto.Role
            };

            await connection.ExecuteAsync(insertQuery, user);
            return true;
        }
    }

    public async Task<string> Login(UserLoginDto userLoginDto)
    {
        var query = "SELECT * FROM [User] WHERE Email = @Email";

        using (var connection = _bookStoreContext.CreateConnection())
        {
            var user = await connection.QueryFirstOrDefaultAsync<UserEntity>(query, new { Email = userLoginDto.Email });

            if (user == null)
            {
                throw new InvalidLoginException("Invalid email");
            }

            if (!BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.Password))
            {
                throw new InvalidLoginException("Invalid password");
            }

            return _authServiceRL.GenerateJwtToken(user);
        }
    }

    public async Task<string> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
    {
        // Check if the provided email exists in the database
        var emailExistsQuery = @"SELECT COUNT(*) FROM [User] WHERE Email = @Email;";

        using (var connection = _bookStoreContext.CreateConnection())
        {
            int count = await connection.QuerySingleAsync<int>(emailExistsQuery, new { Email = forgetPasswordDto.Email });

            if (count == 0)
            {
                throw new NotFoundException($"Email '{forgetPasswordDto.Email}' not found");
            }
        }

        // Generate OTP
        string otp = GenerateOTP();

        // Store OTP in cache
        _cache.Set(forgetPasswordDto.Email, otp, TimeSpan.FromMinutes(5)); // Adjust the expiration time as needed

        // Send OTP to user's email
        await SendOTPEmail(forgetPasswordDto.Email, otp);

        return otp;
    }

    public async Task<bool> ResetPassword(ResetPasswordWithOTPDto resetPasswordWithOTPDto)
    {
        string email = resetPasswordWithOTPDto.Email;
        string storedOtp;

        // Retrieve OTP from cache
        if (!_cache.TryGetValue(email, out storedOtp))
        {
            throw new NotFoundException($"OTP for email '{email}' not found");
        }

        // Check if provided OTP matches stored OTP
        if (resetPasswordWithOTPDto.OTP != storedOtp)
        {
            throw new InvalidOTPException("Invalid OTP");
        }

        // Reset the password in the database
        var query = @"UPDATE [User] SET Password = @Password WHERE Email = @Email;";

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(resetPasswordWithOTPDto.NewPassword);

        using (var connection = _bookStoreContext.CreateConnection())
        {
            await connection.ExecuteAsync(query, new
            {
                Password = hashedPassword,
                Email = resetPasswordWithOTPDto.Email
            });
        }

        // Remove OTP from cache
        _cache.Remove(email);

        return true;
    }

    public bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    private string GenerateOTP()
    {
        // Generate a random 6-digit OTP
        Random rand = new Random();
        return rand.Next(100000, 999999).ToString();
    }

    private async Task SendOTPEmail(string email, string otp)
    {
        // Construct email body with OTP
        var emailBody = $@"
                              <html>
                                   <body>
                                    <p>Hello,</p>
                                    <p>Please use the following OTP to reset your password vaild for next 5 minutes:</p>
                                    <p><strong>{otp}</strong></p>
                                    <p>Thank you!</p>
                                   </body>
                             </html>";

        // Send email
        await _emailServiceRL.SendEmail(email, "Password Reset OTP", emailBody);
    }
}
