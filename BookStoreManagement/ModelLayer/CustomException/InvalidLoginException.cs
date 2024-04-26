namespace ModelLayer.CustomException;

public class InvalidLoginException : Exception
{
    public InvalidLoginException(string message) : base(message)
    {
    }
}
