namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static void SendSMS(SendSMSRequest request)
        {
            // Setup our request
            var webserviceRequest = new Common.Api.ExigoWebService.SendSmsRequest
            {
                CustomerID = request.CustomerID,
                Message    = request.Message
            };
            if (request.Phone.IsNotNullOrEmpty()) webserviceRequest.Phone = request.Phone;


            // Send the request to the web service
            var response = ExigoDAL.WebService().SendSms(webserviceRequest);
        }
    }
}