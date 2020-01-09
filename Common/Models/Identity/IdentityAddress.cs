using ExigoService;
using System.Linq;
using System.Web;

namespace Common.Models
{
    public class IdentityAddress : IIdentityCacheable
    {
        public void Initialize(int customerID)
        {
            var addresses = ExigoDAL.GetCustomerAddresses(customerID);

            this.MainAddress    = addresses.Where(c => c.AddressType == AddressType.Main).FirstOrDefault() as Address;
            this.MailingAddress = addresses.Where(c => c.AddressType == AddressType.Mailing).FirstOrDefault() as Address;
            this.OtherAddress   = addresses.Where(c => c.AddressType == AddressType.Other).FirstOrDefault() as Address;
        }

        public string CacheKey { get; set; }
        public void RefreshCache()
        {
            HttpContext.Current.Cache.Remove(this.CacheKey);
        }

        public Address MainAddress { get; set; }
        public Address MailingAddress { get; set; }
        public Address OtherAddress { get; set; }
    }
}