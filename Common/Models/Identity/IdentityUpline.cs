using ExigoService;
using System;
using System.Linq;
using System.Web;
using Dapper;
using Serilog;

namespace Common.Models
{
    public class IdentityUpline : IIdentityCacheable
    {
        public void Initialize(int customerID)
        {
            dynamic upline = null;


            using (var context = ExigoDAL.Sql())
            {
                try
                {
                    upline = context.Query(@"
                        Select c.EnrollerID,
                            c.CustomerID
                        From Customers c
                        Where c.CustomerID = @customerid
", new
                 {
                    customerid = customerID
                 }).FirstOrDefault();
                       

                }
                catch (Exception e)
                {
                    Log.Error(e,"Error Getting Customer: {Message}", e.Message);
                    return;
                }
            }
            if (upline == null) return;

            if(upline.EnrollerID != null) this.Enroller = ExigoDAL.GetCustomer((int)upline.EnrollerID);
            if (upline.SponsorID != null) this.Sponsor = ExigoDAL.GetCustomer((int)upline.SponsorID);            
        }

        public string CacheKey { get; set; }
        public void RefreshCache()
        {
            HttpContext.Current.Cache.Remove(this.CacheKey);
        }

        public Customer Enroller { get; set; }
        public Customer Sponsor { get; set; }
    }
}