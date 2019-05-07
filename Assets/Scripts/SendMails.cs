using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public static class mono_gmail
{
    public static void SendMail(string sourceMail, string passwordSourceMail, 
                                string destinationMail, string mailSubject, string mailBody)
    {
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(sourceMail);
        mail.To.Add(destinationMail);
        mail.Subject = mailSubject;
        mail.Body = mailBody;

        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new NetworkCredential(sourceMail, passwordSourceMail) as ICredentialsByHost;
        smtpServer.EnableSsl = true;

        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };

        smtpServer.Send(mail);
        Debug.Log("password reset mail sent !");
    }
}