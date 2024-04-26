namespace RepositoryLayer.Entity;

public class UserEntity
{
    public int UserId { get; set; } // Primary Key
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MobileNo { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime AddedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string Role { get; set; }
}

