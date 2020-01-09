using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class PartySummaryViewModel : BasePartyViewModel
    {
        public PartySummaryViewModel()
        {
            this.Orders = new List<Order>();
            this.Guests = new List<Guest>();
        }

        public int OrderCount { get; set; }
        public decimal OrderTotal { get; set; }
        public string HostName { get; set; }
        public List<Order> Orders { get; set; }
        public List<Guest> Guests { get; set; }

        public Party ParentBookingParty { get; set; }
    }
}