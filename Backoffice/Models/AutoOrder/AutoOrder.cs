using ExigoService;
using System;
using System.Collections.Generic;


namespace Backoffice.Models.AutoOrder
{
    public class AutoOrder
    {
        public int AutoOrderID { get; set; }
        public DateTime RunDate { get; set; }
        public decimal Total { get; set; }
        public Address ShippingAddress { get; set; }

        public List<Item> Products { get; set; }

        public string PaymentMethodDescription { get; set; }

        public decimal ShippingAmount { get; set; }
        public string ShipMethodDescription { get; set; }
    }
}