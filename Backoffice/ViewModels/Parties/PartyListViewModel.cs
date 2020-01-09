using Common;
using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class PartyListViewModel
    {
        public PartyListViewModel()
        {
            Parties = new List<Party>();
        }
        public PartyListViewModel(int partyStatusType) : this()
        {
            StatusType = partyStatusType;
            switch (partyStatusType)
            {
                case PartyStatusTypes.Open:
                    StatusDescription = "Open";
                    break;
                case PartyStatusTypes.Closed:
                    StatusDescription = "Closed";
                    break;
                case PartyStatusTypes.Canceled:
                    StatusDescription = "Canceled";
                    break;
                default:
                    break;
            }
        }
        public string StatusDescription { get; set; }
        public int StatusType { get; set; }
        public List<Party> Parties { get; set; }
    }
}