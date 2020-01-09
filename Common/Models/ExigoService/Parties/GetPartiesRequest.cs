using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class GetPartiesRequest
    {
        public GetPartiesRequest()
        {
            //this.PartyStatusID = (int)Common.PartyStatusTypes.Open;
            this.CustomerID = 0;
            this.PartyID = 0;
            this.IncludeHostessDetails = false;
            this.IncludeHostessRewards = false;
            this.IncludeOwnerDetails = false;
        }
        public int PartyStatusID { get; set; }

        public int PartyID { get; set; }
        public int CustomerID { get; set; }
        public int HostessID { get; set; }
        public bool IncludeHostessDetails { get; set; }
        public bool IncludeHostessRewards { get; set; }
        public bool IncludeOwnerDetails { get; set; }
        public bool CheckForHostessOrder { get; set; }

        // Set to true if you want to ignore parties that have not started yet
        public bool ExcludeFutureParties { get; set; }
        // Set to true if you want to ignore parties that have a Close Date value that is less than DateTime.Now
        public bool ExcludeClosedParties { get; set; }
    }
}