using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class ManagePartyViewModel
    {
        public ManagePartyViewModel()
        {
            this.Orders = new List<Order>();
            this.Customers = new List<Customer>();
        }

        public ManagePartyViewModel(Party party)
        {
            this.Orders = new List<Order>();
            this.Customers = new List<Customer>();
            Party = party;
        }

        public Party Party { get; set; }
        public List<Order> Orders { get; set; }
        public List<Customer> Customers { get; set; }
    }
}