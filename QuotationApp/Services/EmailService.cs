using QuotationApp.Data;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace QuotationApp.Services
{
    public class EmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendQuotationEmailAsync(string toEmail, string subject, string body, byte[] attachmentData, string attachmentName)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Quotation App", _smtpSettings.Username));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            builder.Attachments.Add(attachmentName, attachmentData, ContentType.Parse("application/pdf"));
            message.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
