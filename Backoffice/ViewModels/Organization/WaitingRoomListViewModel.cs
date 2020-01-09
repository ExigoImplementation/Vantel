using Backoffice.Models;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backoffice.ViewModels
{
    public class WaitingRoomListViewModel
    {
        public WaitingRoomListViewModel()
        {
            WaitingRoomCustomers = new List<WaitingRoomNode>();
        }

        public List<WaitingRoomNode> WaitingRoomCustomers { get; set; }
        public bool HasWaitingRoomCustomers 
        {
            get { return (this.WaitingRoomCustomers.Count() > 0); }
        }
    }
}