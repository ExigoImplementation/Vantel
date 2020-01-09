using System;
using ExigoService;
using System.Collections.Generic;

namespace ReplicatedSite.ViewModels
{
    public class ManageAutoOrderViewModel
    {
        public ManageAutoOrderViewModel()
        {
            AutoOrder        = new AutoOrder();
            NewCreditCard    = new CreditCard();

            AvailableShipMethods = new List<ShipMethod>();
        }

        public List<ShipMethod> AvailableShipMethods { get; set; }

        public AutoOrder AutoOrder { get; set; }
        public CreditCard NewCreditCard { get; set; }
        public IEnumerable<IItem> AvailableProducts { get; set; }
        public IEnumerable<IPaymentMethod> AvailablePaymentMethods { get; set; }
        public IEnumerable<DateTime> AvailableStartDates { get; set; }
        public bool UsePointAccount { get; set; }
        public bool IsExistingAutoOrder { get { return AutoOrder != null && AutoOrder.AutoOrderID != 0; } }                
    }
}