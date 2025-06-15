using System.Net;
using System.Net.Mail;

public class MailHelper
{
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _smtpPort = 587;
    private readonly string _email;
    private readonly string _password;

    public MailHelper(string email, string password)
    {
        _email = email;
        _password = password;
    }

    public async Task<bool> SendMailAsync(string to, string subject, string body)
    {
        try
        {
            var message = new MailMessage();
            message.From = new MailAddress(_email);
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient(_smtpServer, _smtpPort))
            {
                smtp.Credentials = new NetworkCredential(_email, _password);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
    public async Task<bool> SendMailWithAttachmentAsync(string to, string subject, string body, byte[] attachmentBytes, string attachmentName)
    {
        try
        {
            var message = new MailMessage();
            message.From = new MailAddress(_email);
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            // PDF eklentisini oluştur
            using (var stream = new MemoryStream(attachmentBytes))
            {
                var attachment = new Attachment(stream, attachmentName, "application/pdf");
                message.Attachments.Add(attachment);

                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(_email, _password);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}
