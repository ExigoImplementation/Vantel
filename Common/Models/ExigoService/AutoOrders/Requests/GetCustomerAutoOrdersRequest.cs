namespace ExigoService
{
    public class GetCustomerAutoOrdersRequest
    {
        public int CustomerID { get; set; }
        public bool IncludeCancelledAutoOrders { get; set; }
        public bool IncludePaymentMethods { get; set; }
        public bool IncludeDetails { get; set; }
    }
}