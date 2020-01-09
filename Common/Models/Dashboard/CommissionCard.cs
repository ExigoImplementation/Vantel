using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExigoService;

namespace Common.Models.Dashboard
{
    public class CommissionCard
    {
        public decimal Total { get; set; }
        public string CurrencyCode { get; set; }
        public string CommissionRunDescription { get; set; }

    }
}