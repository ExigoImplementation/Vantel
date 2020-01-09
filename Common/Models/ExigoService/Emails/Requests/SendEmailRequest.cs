using System.Net.Mail;
using Config = Common.GlobalSettings.Emails.Defaults;

namespace ExigoService
{
    public class SendEmailRequest
    {
        public SendEmailRequest()
        {
            this.IsHtml = true;
            this.UseExigoApi = Config.UseExigoApi;
        }

        public SMTPConfiguration SMTPConfiguration { get; set; }

        public bool IsHtml { get; set; }
        public MailPriority Priority { get; set; }

        public string[] To { get; set; }
        public string[] ReplyTo { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        // Flag to determine if we should use the Exigo WebService SendEmail call
        public bool UseExigoApi { get; set; }
    }
}
