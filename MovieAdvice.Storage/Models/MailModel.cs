using MovieAdvice.Storage.Enums;
using System.Net.Mail;

namespace MovieAdvice.Storage.Models
{
    public class MailModel
    {
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string To { get; set; }
        public EmailSettingModel EmailSetting { get; set; }

        public TemplateType TemplateType { get; set; }

        public object Data { get; set; }

        public MailMessage GetMailMessage()
        {
            var mailMessage = new MailMessage
            {
                Subject = this.Subject,
                Body = this.Body,
                From = new MailAddress(this.EmailSetting?.EmailAddress)
            };
            mailMessage.To.Add(To);
            mailMessage.IsBodyHtml = true;
            return mailMessage;
        }
    }
}
