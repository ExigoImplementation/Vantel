using System;
using System.Collections.Generic;

namespace ExigoService
{
    public class GetNewestDistributorsRequest : DataRequest
    {
        public GetNewestDistributorsRequest()
            : base()
        {
            MaxLevel = 10;
            CustomerStatuses = new List<int>();
            CustomerTypes = new List<int>();
            Days = 0;
        }

        public int CustomerID { get; set; }
        public int MaxLevel { get; set; }
        public List<int> CustomerStatuses { get; set; }
        public List<int> CustomerTypes { get; set; }
        public int Days { get; set; }

    }
}