using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backoffice.ViewModels
{
    public class PartyOrderViewModel
    {
        public PartyOrderViewModel()
        {
            Order = new PartyOrder();
            NewCreditCard = new CreditCard();
            AvailableShipMethods = new List<ShipMethod>();
        }

        public IEnumerable<IShipMethod> AvailableShipMethods { get; set; }
        public PartyOrder Order { get; set; }
        public CreditCard NewCreditCard { get; set; }
        public List<Item> AvailableProducts { get; set; }
        public IEnumerable<IPaymentMethod> AvailablePaymentMethods { get; set; }
    }
}