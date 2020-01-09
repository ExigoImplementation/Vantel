using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class PartySearchRequest
    {
        public PartySearchRequest()
        {
            this.PartyStatusID = (int)Common.PartyStatusTypes.Open;
        }
        public int PartyStatusID { get; set; }

        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ZipCode { get; set; }
        public DateTime? EventStart { get; set; }
        public int PartyID { get; set; }

        public bool ShowOpenPartiesOnly { get; set; }
    }
}