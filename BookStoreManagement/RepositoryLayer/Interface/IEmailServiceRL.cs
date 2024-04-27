namespace RepositoryLayer.Interface;

public interface IEmailServiceRL
{
    public Task<bool> SendEmail(string to, string subject, string htmlMessage);
}
