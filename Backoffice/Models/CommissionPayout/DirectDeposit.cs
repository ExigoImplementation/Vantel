using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExigoService;

namespace Backoffice.Models.CommissionPayout
{
    public class CommissionPayout
    {
        public string NameOnAccount { get; set; }
        public string BankName      { get; set; }
        public string AccountNumber { get; set; }
        public string RoutingNumber { get; set; }

        public bool IsComplete
        {
            get
            {
                if (string.IsNullOrEmpty(NameOnAccount)) return false;
                if (string.IsNullOrEmpty(BankName)) return false;
                if (string.IsNullOrEmpty(AccountNumber)) return false;
                if (string.IsNullOrEmpty(RoutingNumber)) return false;

                return true;
            }
        }
    }
}