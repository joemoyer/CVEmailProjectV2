using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace EmailAPI
{
    public class EmailSender : IEmailSender
    {
        private string mail = "";
        private string pw = "";
        private DBController db = null;
        
        public EmailSender(IConfiguration configuration){
            this.mail = configuration["HostEmail"];
            this.pw = configuration["HostKey"];
            this.db = new DBController(configuration);
        }

        public EmailSender(){}
        public Task SendEmailAsync(string email, string subject, string message) 
        {
            db.addEmailAttempt(email,subject,message);
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            MailMessage emailmessage = new MailMessage();
            emailmessage.From = new MailAddress(mail);
            emailmessage.Subject = subject;
            emailmessage.To.Add(new MailAddress(email));
            emailmessage.Body = message;
            emailmessage.IsBodyHtml = true;

            return client.SendMailAsync(emailmessage);

            //smtpClient.Send(message);
        }
    }
}