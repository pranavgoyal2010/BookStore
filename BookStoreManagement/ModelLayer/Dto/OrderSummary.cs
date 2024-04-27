namespace ModelLayer.Dto;

public class OrderSummary
{
    public int OrderId { get; set; }
    public int BookId { get; set; }
    public int Quantity { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string BookImage { get; set; }
    //public decimal Amount { get; set; }
}
