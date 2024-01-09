namespace ProductManagement.Infrastructure.Email;

public interface ISmtpClientHelper
{
    Task SendEmail(string to, string subject, string body, CancellationToken cancellationToken);
}
