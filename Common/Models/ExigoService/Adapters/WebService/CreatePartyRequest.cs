using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Api.ExigoWebService
{
    public partial class CreatePartyRequest
    {
        public CreatePartyRequest() { }
        public CreatePartyRequest(ExigoService.Party party)
        {
            var model = new CreatePartyRequest();
            if (party == null) return;

            PartyType = party.PartyType;
            PartyStatusType = party.PartyStatusType;
            HostID = party.HostID;
            DistributorID = party.CustomerID;
            StartDate = Convert.ToDateTime(party.StartDate);
            CloseDate = party.CloseDate;
            Description = party.Description;
            EventStart = party.EventStart;
            EventEnd = party.EventEnd;
            LanguageID = Convert.ToInt32(party.LanguageID);
            Information = party.Information;

            Address = (PartyAddress)party.Address;
        }
        public CreatePartyRequest(ExigoService.CreatePartyRequest party)
        {
            var model = new CreatePartyRequest();
            if (party == null) return;

            PartyType = 1;
            PartyStatusType = 1;
            HostID = party.HostID;
            DistributorID = party.CustomerID;
            
            if(party.ParentPartyID > 0)
            {
                BookingPartyID = party.ParentPartyID;
            }

            var partyStartDate = new DateTime(party.EventStartDate.Year, party.EventStartDate.Month, party.EventStartDate.Day, party.EventStartTime.Hour, party.EventStartTime.Minute, party.EventStartTime.Second);
            StartDate = partyStartDate;
            EventStart = partyStartDate;

            // Some logic to ensure that the end date is set to 30 days after the 
            if (party.EventEndDate < partyStartDate)
            {
                party.EventEndDate = partyStartDate.AddDays(GlobalSettings.Parties.PartyOpenDays);
            }

            var endDate = new DateTime(party.EventEndDate.Year, party.EventEndDate.Month, party.EventEndDate.Day, party.EventEndTime.Hour, party.EventEndTime.Minute, party.EventEndTime.Second);
            CloseDate = endDate;
            EventEnd = endDate;
            Description = (party.Description.IsNullOrEmpty()) ? " " : party.Description;
            LanguageID = 0;

            // Sales Goal
            Field1 = party.SalesGoal.ToString();
            

            Address = new PartyAddress
            {
                Address1 = party.Address1,
                Address2 = party.Address2,
                City = party.City,
                State = party.State,
                Zip = party.Zip,
                Country = party.Country
            };
        }
    }
}