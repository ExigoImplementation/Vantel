using Common.Api.ExigoWebService;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Common.Utilities
{
    public class Identity
    {
        public static KeyValuePair<bool, List<string>> IdentityCheck (int customerID )
        {
            var nullList = new List<string>();

            var cust = ExigoDAL.GetCustomer(customerID);


            if (cust.MainAddress == null || string.IsNullOrEmpty(cust.MainAddress.Country))
            {
                nullList.Add("Main Country");
            }


            if (nullList.Count() <= 0)
            {
                return new KeyValuePair<bool, List<string>>(true, nullList);
            }
            else
            {
                return new KeyValuePair<bool, List<string>>(false, nullList);
            }
        
          
        }
    }
}