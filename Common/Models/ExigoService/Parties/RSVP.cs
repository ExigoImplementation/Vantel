using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class RSVPModel
    {
        public RSVPModel()
        { }

        public RSVPModel(int partyID)
        {
            this.PartyID = partyID;
        }

        public int PartyID { get; set; }
        public Guest Guest { get; set; }
        public Party Party { get; set; }
        public string PartyRSVPUrl { get; set; }
        public string OwnerWebAlias { get; set; }
        public string HostEmail { get; set; }
        public string HostPhone { get; set; }

        public bool HasEmail { get { return this.HostEmail.IsNotNullOrEmpty(); } }
        public bool HasPhone { get { return this.HostEmail.IsNotNullOrEmpty(); } }
    }
}