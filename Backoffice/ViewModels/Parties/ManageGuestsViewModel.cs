using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backoffice.ViewModels
{
    public class ManageGuestsViewModel : BasePartyViewModel
    {
        public ManageGuestsViewModel() {}
        public ManageGuestsViewModel(int partyID)
        {
            Guest = new Guest();
            PartyID = partyID;
        }

        public Guest Guest { get; set; }
        public int PartyID { get; set; }
        public int HostessID { get; set; }
        public List<Customer> Customers { get; set; }
        public bool HasCustomers()
        {
            return this.Customers != null && this.Customers.Count() > 0;
        }
    }
}