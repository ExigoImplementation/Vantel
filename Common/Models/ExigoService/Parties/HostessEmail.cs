using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class HostessEmailModel
    {
        public HostessEmailModel()
        { }

        public HostessEmailModel(int partyID)
        {
            this.PartyID = partyID;
        }

        public int PartyID { get; set; }
        // Customer ID of Hostess record (Exigo Customer)
        public int CustomerID { get; set; }
        public Party Party { get; set; }
        public string HostessLoginUrl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ReplicatedSiteHomePage { get; set; }
    }
}