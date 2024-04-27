namespace RepositoryLayer.Entity;

public class OrderItemEntity
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int BookId { get; set; }
    public int Quantity { get; set; }
}
