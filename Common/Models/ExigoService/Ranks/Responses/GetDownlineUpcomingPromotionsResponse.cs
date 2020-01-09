using System.Collections.Generic;

namespace ExigoService
{
    public class GetDownlineUpcomingPromotionsResponse
    {
        public IEnumerable<CustomerRankScore> CustomerRankScores { get; set; }
        public int TotalCount { get; set; }
    }
}
