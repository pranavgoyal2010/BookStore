namespace ModelLayer.Dto;

public class AddBookDto
{
    public string BookName { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
    public string BookImage { get; set; }
    public int Quantity { get; set; }
}

public class UpdateBookDto
{
    public string BookName { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public decimal Price { get; set; }
    public string BookImage { get; set; }
    public int Quantity { get; set; }
}
