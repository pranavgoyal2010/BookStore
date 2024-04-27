using BusinessLayer.Interface;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service;

public class EmailServiceBL : IEmailServiceBL
{
    private readonly IEmailServiceRL _emailServiceRL;

    public EmailServiceBL(IEmailServiceRL emailServiceRL)
    {
        _emailServiceRL = emailServiceRL;
    }

    public async Task<bool> SendEmail(string to, string subject, string htmlMessage)
    {
        return await _emailServiceRL.SendEmail(to, subject, htmlMessage);
    }
}
