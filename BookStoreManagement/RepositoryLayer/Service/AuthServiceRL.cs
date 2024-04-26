using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Service;

public class AuthServiceRL : IAuthServiceRL
{
    private readonly IConfiguration _configuration;
    public AuthServiceRL(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(UserEntity userEntity)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userEntity.UserId.ToString()), // Include user ID as a claim
                                                                      
                new Claim(ClaimTypes.Email, userEntity.Email), // Include user Email as a claim

                new Claim(ClaimTypes.Role, userEntity.Role) // Include user Role as a claim
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
