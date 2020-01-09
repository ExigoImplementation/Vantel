using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class PartyGuestList
    {
        public PartyGuestList()
        {
            this.Guests = new List<Guest>();
        }

        public int PartyID { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public List<Guest> Guests { get; set; }
    }
}