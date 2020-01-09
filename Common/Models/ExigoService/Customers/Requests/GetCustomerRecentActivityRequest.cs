using System;

namespace ExigoService
{
    public class GetCustomerRecentActivityRequest : DataRequest
    {
        public GetCustomerRecentActivityRequest()
            : base()
        {
        }

        public int CustomerID { get; set; }
        public DateTime? StartDate { get; set; }
    }
}