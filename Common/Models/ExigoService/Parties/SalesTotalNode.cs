using ExigoService;
using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class SalesTotalNode
    {
        public int WeekID { get; set; }
        public int PartyID { get; set; }
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerID { get; set; }
        public decimal Total { get; set; }
    }
}