using Config = Common.GlobalSettings.Emails.Defaults;
using System.Net.Mail;

namespace ExigoService
{
    public class Email
    {
        //properties
        private SendEmailRequest request { get; set; }

        //Constructor
        public Email()
        {
            request = new SendEmailRequest()
            {
                To = Config.To,
                ReplyTo = Config.ReplyTo,
                Subject = Config.Subject,
                Body = Config.Body,
                From = Config.From,
                IsHtml = Config.IsHtml,
                Priority = Config.Priority,
                UseExigoApi = Config.UseExigoApi,
            };
        }

        //getters
        public string[] To() { return request.To; }
        public string[] ReplyTo() { return request.ReplyTo; }
        public string From() { return request.From; }
        public string Subject() { return request.Subject; }
        public string Body() { return request.Body; }
        public bool IsHtml() { return request.IsHtml; }
        public bool UseExigoApi() { return request.UseExigoApi; }
        public MailPriority Priority() { return request.Priority; }

        //setters
        public Email To(params string[] to) { request.To = to; return this; }
        public Email To(string to) { request.To = new string[] { to }; return this; }
        public Email ReplyTo(params string[] replyto) { request.ReplyTo = replyto; return this; }
        public Email ReplyTo(string replyto) { request.ReplyTo = new string[] { replyto }; return this; }
        public Email From(string from) { request.From = from; return this; }
        public Email Subject(string subject) { request.Subject = subject; return this; }
        public Email Body(string body) { request.Body = body; return this; }
        public Email IsHtml(bool isHtml) { request.IsHtml = isHtml; return this; }
        public Email UseExigoApi(bool use) { request.UseExigoApi = use; return this; }
        public Email Priority(MailPriority priority) { request.Priority = priority; return this; }

        //actions
        public bool Send()
        {
            try
            {
                ExigoDAL.SendEmail(request);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}