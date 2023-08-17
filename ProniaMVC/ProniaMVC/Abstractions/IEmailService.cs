namespace ProniaMVC.Abstractions
{
    public interface IEmailService
    {
        void SendMail(string mailTo, string subject, string body, bool IsBodyHtml = false);
    }
}
