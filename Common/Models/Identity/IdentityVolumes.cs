using ExigoService;
using System.Web;

namespace Common.Models
{
    public class IdentityVolumes : IIdentityCacheable
    {
        public void Initialize(int customerID)
        {
            var weeklyVolumes = ExigoDAL.GetCustomerVolumes(new GetCustomerVolumesRequest
            {
                CustomerID = customerID,
                PeriodTypeID = PeriodTypes.Weekly
            });

            this.WeeklyVolumes = weeklyVolumes;
        }

        public string CacheKey { get; set; }
        public void RefreshCache()
        {
            HttpContext.Current.Cache.Remove(this.CacheKey);
        }

        public VolumeCollection WeeklyVolumes { get; set; }
    }
}