using Common.Api.ExigoWebService;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static void SendEmail(SendEmailRequest request)
        {

            var sendEmailRequest = new Common.Api.ExigoWebService.SendEmailRequest
            {
                MailFrom = request.From,
                Subject = request.Subject,
                Body = request.Body
            };

            // Send the emails
            var tasks = new List<Task>();
            foreach (var email in request.To)
            {
                tasks.Add(
                Task.Factory.StartNew(() =>
                {
                    sendEmailRequest.MailTo = email;
                    ExigoDAL.WebService().SendEmail(sendEmailRequest);
                }));
            }
            Task.WaitAll(tasks.ToArray());
        }
    }
}