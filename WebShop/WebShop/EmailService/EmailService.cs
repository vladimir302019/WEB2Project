using MimeKit;

namespace WebShop.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailService(EmailConfiguration emailConfiguration)
        {
            _emailConfig = emailConfiguration;
        }

        public async Task SendEmail(Message message)
        {
            //create message
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Vladimir", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Body };

            //send message
            using(var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.CheckCertificateRevocation = false;
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, MailKit.Security.SecureSocketOptions.Auto);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.Username,_emailConfig.Password);

                    await client.SendAsync(emailMessage);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
