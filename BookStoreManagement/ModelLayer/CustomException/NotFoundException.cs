namespace ModelLayer.CustomException;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
