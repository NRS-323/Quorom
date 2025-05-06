using MailKit.Net.Smtp;
using MimeKit;
using Quorom.DbTables;
using Quorom.Repositories;

namespace Quorom.Services
{
    public class EmailWithMailKit : IEmailWithMailKit
    {
        private readonly INotificationLogRepository _log;
        public EmailWithMailKit (INotificationLogRepository log)
        {
            _log = log;
        }
        public async Task<bool> SendMailViaList(List<string> toEmail, string subject, string message, Guid NotificationGroupId, string user)
        {
            var email = new MimeMessage();

            //email.From.Add(new MailboxAddress("QUOROM Administrator-No Reply", "umbonosystem@gov.tt"));
            email.From.Add(new MailboxAddress("QUOROM Administrator-No Reply", "nkosialexander@gmail.com"));
            toEmail.ForEach(address => email.To.Add(new MailboxAddress(address, address)));

            email.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            email.Body = bodyBuilder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                //smtp.Connect("mailrelay.gov.tt", 25, false);
                smtp.Connect("smtp.gmail.com", 587, false);
                smtp.Capabilities &= ~SmtpCapabilities.Pipelining;

                // Note: only needed if the SMTP server requires authentication
                //smtp.Authenticate(UmbonoGeneral.UmbonoConstants.umbonoMailUser, UmbonoGeneral.UmbonoConstants.umbonoMailPassword);
                smtp.Authenticate("nkosialexander@gmail.com", "kcbayezisybkyvch"); //app password from Google
                
                smtp.Send(email);
                var log = new NotificationLog()
                {
                    NotificationType = "Mail List",
                    NotificationGroupId = NotificationGroupId,
                    RecipientEmail = null,
                    CreatedByUserId = user,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log);
                smtp.Disconnect(true);
            }
            return true;
        }
        public async Task<bool> SendMailViaUser(string toEmail, string subject, string message, string user)
        {
            var email = new MimeMessage();

            //email.From.Add(new MailboxAddress("QUOROM Administrator-No Reply", "umbonosystem@gov.tt"));
            email.From.Add(new MailboxAddress("QUOROM Administrator-No Reply", "nkosialexander@gmail.com"));
            email.To.Add(new MailboxAddress(toEmail, toEmail));

            email.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            email.Body = bodyBuilder.ToMessageBody();

            using (var smtp = new SmtpClient())
            {
                //smtp.Connect("mailrelay.gov.tt", 25, false);
                smtp.Connect("smtp.gmail.com", 587, false);
                smtp.Capabilities &= ~SmtpCapabilities.Pipelining;

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate("nkosialexander@gmail.com", "kcbayezisybkyvch"); //app password from Google
                //smtp.Authenticate(UmbonoGeneral.UmbonoConstants.umbonoMailUser, UmbonoGeneral.UmbonoConstants.umbonoMailPassword);

                smtp.Send(email);

                var log = new NotificationLog()
                {
                    NotificationType = "Recipient",
                    NotificationGroupId = null,
                    RecipientEmail = toEmail,
                    CreatedByUserId = user,
                    CreatedOnDateTime = DateTime.Now
                };
                await _log.AddLogAsync(log);               
                smtp.Disconnect(true);
            }
            return true;
        }
    }
    public interface IEmailWithMailKit
    {
        Task<bool> SendMailViaList(List<string> toEmail, string subject, string message, Guid NotificationGroupId, string user);
        Task<bool> SendMailViaUser(string toEmail, string subject, string message, string user);
    }
}


