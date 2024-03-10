using MailKit.Security;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Tournament.Extensions
{
    public class EmailService
    {
        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Maras Kamu Turnuva", "info@maraskamuturnuva.com")); // Gönderenin bilgilerini ekleyin
            message.To.Add(new MailboxAddress("", to)); // Alıcının e-posta adresini ekleyin
            message.Subject = subject;

            // E-posta gövdesini HTML olarak ayarlayın
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body; // HTML içeriği buraya eklenir

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("srvm13.trwww.com", 465, SecureSocketOptions.SslOnConnect); // SMTP sunucu bilgilerini ekleyin
                await client.AuthenticateAsync("info@maraskamuturnuva.com", "Ed4b122ff."); // SMTP kimlik bilgilerini ekleyin
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            return true;
        }
    }
}
