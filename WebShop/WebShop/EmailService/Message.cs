using Microsoft.Extensions.Diagnostics.HealthChecks;
using MimeKit;

namespace WebShop.EmailService
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public Message(IEnumerable<string> to, string subject, string body)
        {
            To = new List<MailboxAddress>();
            Subject = subject;
            Body = body;
            To.AddRange(to.Select(x => new MailboxAddress(x,x)));
        }
    }
}
