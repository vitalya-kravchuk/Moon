using System.Configuration;
using System.Net.Mail;
using System;

public class Mail
{
    SmtpClient smtpClient;
    MailMessage mailMsg;

    public Mail()
    {
        smtpClient = new SmtpClient();
        smtpClient.Timeout = 5000;
    }

    public void Send(string from, string fromName, string to, string subject, string body)
    {
        mailMsg = new MailMessage();
        if (!string.IsNullOrEmpty(from))
        {
            mailMsg.From = new MailAddress(from, fromName);
        }
        mailMsg.To.Add(to);
        mailMsg.Subject = subject;
        mailMsg.Body = body;
        try
        {
            smtpClient.Send(mailMsg);
        }
        catch (Exception e)
        {
            Logger.Log.ErrorFormat("{0} => {1} \r\n", to, e.Message);
        }
    }

    public void Send(string to, string subject, string body)
    {
        Send("", "", to, subject, body);
    }
}
