using System.Collections.Generic;

namespace ExigoService
{
    public class GetCustomerVolumesRequest
    {
        public GetCustomerVolumesRequest()
            : base()
        {
        }

        public int CustomerID { get; set; }
        public int PeriodTypeID { get; set; }
        public int? PeriodID { get; set; }      
        
        public List<int> VolumesToFetch { get; set; }  
    }
}