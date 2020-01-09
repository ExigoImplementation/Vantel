using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class WaitingRoomViewModel
    {
        public List<WaitingRoomCustomerViewModel> Customers { get; set; }
        public List<WaitingRoomSponsorViewModel> Sponsors { get; set; }
    }
}