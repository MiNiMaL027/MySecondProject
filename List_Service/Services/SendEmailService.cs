using List_Service.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;

namespace List_Service.Services
{
    public class SendEmailService : ISendEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("List Supports", "todolist@meta.ua"));
            emailMessage.To.Add(new MailboxAddress("user", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.meta.ua", 465, true);
                await client.AuthenticateAsync("todolist@meta.ua", "Andriy321");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
