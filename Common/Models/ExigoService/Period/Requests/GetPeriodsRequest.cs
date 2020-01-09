using System.Collections.Generic;

namespace ExigoService
{
    public class GetPeriodsRequest
    {
        public int? CustomerID { get; set; }
        public int PeriodTypeID { get; set; }
        public List<int> PeriodIDs { get; set; }
        public bool OnlyAcceptedPeriods { get; set; }
    }
}