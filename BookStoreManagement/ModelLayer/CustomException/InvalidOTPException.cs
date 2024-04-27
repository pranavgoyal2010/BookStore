namespace ModelLayer.CustomException;

public class InvalidOTPException : Exception
{
    public InvalidOTPException(string message) : base(message)
    {
    }
}
