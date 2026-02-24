using Harfien.Domain.Interface_Repository.Repositories;
using MailKit.Net.Smtp;
using MimeKit;


namespace Harfien.Infrastructure.Repositories
{
    public class EmailSender : IEmailService
    {
        public async Task SendAsync(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("wwork1242@gmail.com"));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = body
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                "smtp.gmail.com",
                587,
                MailKit.Security.SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(
                "wwork1242@gmail.com",
                "tvgv hqra xtbv diaf"
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
