namespace List_Service.Interfaces
{
    public interface ISendEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
