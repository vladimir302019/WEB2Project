namespace WebShop.EmailService
{
    public interface IEmailService
    {
        Task SendEmail(Message message);
    }
}
