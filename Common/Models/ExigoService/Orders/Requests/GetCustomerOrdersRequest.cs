using System;

namespace ExigoService
{
    public class GetCustomerOrdersRequest : DataRequest
    {
        public GetCustomerOrdersRequest()
            : base()
        {
            OrderStatuses = new int[0];
            OrderTypes = new int[0];
        }

        public int CustomerID { get; set; }
        public int? OrderID { get; set; }
        public int[] OrderStatuses { get; set; }
        public int[] OrderTypes { get; set; }
        public DateTime? StartDate { get; set; }

        public bool IncludeOrderDetails { get; set; }
        public bool IncludePayments { get; set; }

        public int LanguageID { get; set; }
        public int PartyID { get; set; }
    }
}