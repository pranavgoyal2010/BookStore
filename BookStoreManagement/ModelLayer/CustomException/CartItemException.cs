namespace ModelLayer.CustomException;

public class CartItemException : Exception
{
    public CartItemException(string message) : base(message) { }
}
