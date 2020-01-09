using Backoffice.Models;
using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class CreatePartyViewModel
    {
        public CreatePartyViewModel()
        {
           
            CreatePartyRequest = new CreatePartyRequest();
        }

        public CreatePartyRequest CreatePartyRequest { get; set; }

        // If we are dealing with an existing Party
        public int PartyID { get; set; }
        // If we are creating a Party within another Party
        public int ParentPartyID { get; set; }
        public string ParentPartyDescription { get; set; }
    }
}