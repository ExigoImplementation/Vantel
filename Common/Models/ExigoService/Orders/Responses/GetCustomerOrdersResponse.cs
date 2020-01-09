using System;
using System.Collections.Generic;

namespace ExigoService
{
    public class GetCustomerOrdersResponse
    {
        public GetCustomerOrdersResponse()
        {
            Orders = new List<Order>();
        }
        public GetCustomerOrdersResponse(int rowCount)
        {
            RowCount = rowCount;
        }

        public List<Order> Orders { get; set; }
        public int OrderCount { get; set; }

        // Pagination
        public int Page { get; set; }
        public int RowCount { get; set; }
    }
}