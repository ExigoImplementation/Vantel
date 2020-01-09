using ExigoService;
using System.Web;

namespace Common.Models
{
    public class IdentityRanks : IIdentityCacheable
    {
        public void Initialize(int customerID)
        {
            var ranks = ExigoDAL.GetCustomerRanks(new GetCustomerRanksRequest
            {
                CustomerID   = customerID,
                PeriodTypeID = PeriodTypes.Weekly
            });

            this.CurrentPeriodRank          = ranks.CurrentPeriodRank;
            this.HighestPaidRankInAnyPeriod = ranks.HighestPaidRankInAnyPeriod;
            this.HighestPaidRankUpToPeriod  = ranks.HighestPaidRankUpToPeriod;
        }

        public string CacheKey { get; set; }
        public void RefreshCache()
        {
            HttpContext.Current.Cache.Remove(this.CacheKey);
        }

        public Rank CurrentPeriodRank { get; set; }
        public Rank HighestPaidRankInAnyPeriod { get; set; }
        public Rank HighestPaidRankUpToPeriod { get; set; }
    }
}