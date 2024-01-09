using MailKit.Net.Smtp;
using MimeKit.Text;
using MimeKit;

namespace ProductManagement.Infrastructure.Email;

/// <summary>
/// Helper for sending emails
/// TODO:
///     Extract this to its own library together with IProductsEmailService
/// </summary>
public class SmtpClientHelper : ISmtpClientHelper
{
    //TODO: Get all values from keyvault or similar secret store
    private readonly string smtpHost;
    private readonly int smtpPort;
    private readonly string smtpUsername;
    private readonly string smtpPassword;

    public SmtpClientHelper()
    {
        
    }   

    public async Task SendEmail(string to, string subject, string body, CancellationToken cancellationToken)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Your Name", "your-email@example.com"));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;

        message.Body = new TextPart(TextFormat.Html)
        {
            Text = body,
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(smtpHost, smtpPort, false, cancellationToken);
            await client.AuthenticateAsync(smtpUsername, smtpPassword, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }
    }

}
