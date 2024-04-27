namespace RepositoryLayer.Entity;

public class OrderEntity
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Address { get; set; }

}
