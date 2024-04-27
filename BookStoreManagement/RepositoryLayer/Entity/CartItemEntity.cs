namespace RepositoryLayer.Entity;

public class CartItemEntity
{
    public int CartItemId { get; set; }
    public int CartId { get; set; }
    public int BookId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

}
