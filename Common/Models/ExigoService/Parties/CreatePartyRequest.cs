using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExigoService
{

    public class CreatePartyRequest
    {
        public CreatePartyRequest()
        { }
        public CreatePartyRequest(Party party)
        {
            CustomerID = party.CustomerID;
            HostID = party.HostID;
            PartyID = party.PartyID;
            StartDate = (party.StartDate.CanBeParsedAs<DateTime>()) ? Convert.ToDateTime(party.StartDate) : new DateTime();
            CloseDate = (party.CloseDate.CanBeParsedAs<DateTime>()) ? Convert.ToDateTime(party.CloseDate) : new DateTime();

            EventStartDate = (party.EventStart.CanBeParsedAs<DateTime>()) ? Convert.ToDateTime(party.EventStart) : new DateTime();
            EventStartTime = (party.EventStart.CanBeParsedAs<DateTime>()) ? Convert.ToDateTime(party.EventStart) : new DateTime();
            EventEndDate = (party.EventEnd.CanBeParsedAs<DateTime>()) ? Convert.ToDateTime(party.EventEnd) : new DateTime();
            EventEndTime = (party.EventEnd.CanBeParsedAs<DateTime>()) ? Convert.ToDateTime(party.EventEnd) : new DateTime();

            Description = party.Description;

            if (party.Address != null && party.Address.IsComplete)
            {
                Address1 = party.Address.Address1;
                Address2 = party.Address.Address2;
                City = party.Address.City;
                State = party.Address.State;
                Zip = party.Address.Zip;
                Country = party.Address.Country;
            }

            HostID = party.HostID;
            HostFirstName = party.HostFirstName;
            HostLastName = party.HostLastName;
            HostPhone = party.HostPhone;

        }

        public int CustomerID { get; set; }

        //[Display(ResourceType = typeof(Resources.Common), Description = "Theme")]
        public string Theme { get; set; }

        public string Description { get; set; }

        // Created date and date the party status is set to closed
        public DateTime StartDate { get; set; }
        public DateTime CloseDate { get; set; }


        // Start and End Dates for the Party itself
        [Required]
        public DateTime EventStartDate { get; set; }
        public DateTime EventStartTime { get; set; }

        [Required]
        public DateTime EventEndDate { get; set; }
        public DateTime EventEndTime { get; set; }

        public string TimeZone { get; set; }

        public decimal SalesGoal { get; set; }

        public bool IsVirtualOnly { get; set; }
        public bool UseOwnAddress { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Zip { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public int HostID { get; set; }

        // Populated if we are dealing with an existing Party
        public int PartyID { get; set; }
        // Populated if we are dealing with a Party that is being booked under an existing Party
        public int ParentPartyID { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string HostFirstName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string HostLastName { get; set; }

        public string HostEmail { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string HostPhone { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string HostAddress1 { get; set; }
        public string HostAddress2 { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string HostCity { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string HostState { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string HostZip { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string HostCountry { get; set; }
    }
}