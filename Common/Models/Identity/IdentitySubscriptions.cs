using ExigoService;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.Models
{
    public class IdentitySubscriptions : IIdentityCacheable
    {
        public void Initialize(int customerID)
        {
            var subscriptions = ExigoDAL.GetCustomerSubscriptions(customerID);

            this.AllSubscriptions = subscriptions.ToList();
        }

        public string CacheKey { get; set; }
        public void RefreshCache()
        {
            HttpContext.Current.Cache.Remove(this.CacheKey);
        }

        public List<CustomerSubscription> AllSubscriptions { get; set; }
    }
}