namespace ModelLayer.CustomException;

public class InvalidEmailFormatException : Exception
{
    public InvalidEmailFormatException(string message) : base(message) { }
}
