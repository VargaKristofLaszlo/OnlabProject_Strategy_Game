namespace BackEnd.Services.Interfaces
{
    public interface IMailService
    {
        void SendEmail(string toEmail, string subject, string content);
    }
}
