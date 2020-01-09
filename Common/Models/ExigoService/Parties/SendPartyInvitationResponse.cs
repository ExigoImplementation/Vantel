using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class SendPartyInvitationResponse
    {
        public SendPartyInvitationResponse()
        {
            this.FailedEmails = new List<string>();
        }
        public SendPartyInvitationResponse(int partyID)
        {
            this.PartyID = partyID;
            this.FailedEmails = new List<string>();
        }

        public int PartyID { get; set; }
        public List<string> FailedEmails { get; set; }
    }
}