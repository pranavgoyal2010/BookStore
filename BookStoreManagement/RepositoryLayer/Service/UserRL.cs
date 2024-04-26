using Dapper;
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

    public UserRL(BookStoreContext bookStoreContext)
    {
        _bookStoreContext = bookStoreContext;
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

    public async Task<bool> Login(UserLoginDto userLoginDto)
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

            return true;
        }
    }


    public bool IsValidEmail(string email)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }
}
