using ExigoService;
using System;

namespace Backoffice.ViewModels
{
    public class WaitingRoomCustomerViewModel
    {
        public DateTime PlacementExpirationDate { get; set; }
        public WaitingRoomNode Customer { get; set; }
    }
}