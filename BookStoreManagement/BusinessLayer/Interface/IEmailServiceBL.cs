namespace BusinessLayer.Interface;

public interface IEmailServiceBL
{
    public Task<bool> SendEmail(string to, string subject, string htmlMessage);
}
