using System.Collections.Generic;

namespace ExigoService
{
    public class GetCustomerRealTimeCommissionsRequest
    {
        public int CustomerID { get; set; }     
        public bool GetPeriodVolumes { get; set; }
        public List<int> VolumesToFetch { get; set; }
    }
}