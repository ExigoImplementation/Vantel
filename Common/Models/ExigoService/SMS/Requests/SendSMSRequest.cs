namespace ExigoService
{
    public class SendSMSRequest
    {
        public int CustomerID { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
    }
}
