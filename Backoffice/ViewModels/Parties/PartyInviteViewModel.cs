using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class PartyInviteViewModel
    {
        public PartyInviteViewModel()
        {
            this.Guests = new List<Guest>();
        }

        public PartyInviteViewModel(Party party)
        {
            this.Guests = new List<Guest>();
            Party = party;
        }

        public Party Party { get; set; }
        public List<Guest> Guests { get; set; }
    }
}