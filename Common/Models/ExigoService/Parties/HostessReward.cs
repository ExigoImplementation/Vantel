using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class HostessReward
    {
        public int PartyID { get; set; }
        public decimal SalesTotal { get; set; }
        public decimal FreeProductAmount { get; set; }
        public decimal FreeProductPercentage { get; set; }
        public int HalfPricedItems { get; set; }

        public decimal GetFreeProductAmount(decimal partyTotal)
        {
            if (this.FreeProductAmount > 0)
            {
                return this.FreeProductAmount;
            }
            else
            {
                return this.FreeProductPercentage * partyTotal;
            }
        }
    }
}