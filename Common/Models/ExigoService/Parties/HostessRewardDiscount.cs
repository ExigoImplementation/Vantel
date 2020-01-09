using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class HostessRewardDiscount
    {
        public string ItemCode { get; set; }
        public decimal DiscountPrice { get; set; }
        public bool IsFreeDiscount { get; set; }
    }
}