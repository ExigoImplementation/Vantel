using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Api.ExigoWebService
{
    public partial class PartyResponse
    {
        public static explicit operator ExigoService.Party(PartyResponse party)
        {
            var model = new ExigoService.Party();
            if (party == null) return model;

            model.PartyID         = party.PartyID;
            model.PartyType       = party.PartyType;
            model.PartyStatusType = party.PartyStatusType;
            model.HostID          = party.HostID;
            model.CustomerID      = party.DistributorID;
            model.StartDate       = party.StartDate;
            model.CloseDate       = party.CloseDate;
            model.Description     = party.Description;
            model.EventStart      = party.EventStart;
            model.EventEnd        = party.EventEnd;
            model.LanguageID      = party.LanguageID;

            model.Information     = party.Information;

            model.CurrentSales = (party.Field1.CanBeParsedAs<decimal>()) ? Convert.ToDecimal(party.Field1) : 0;
            model.MasterOrderID = party.Field3.CanBeParsedAs<int>() ? Convert.ToInt32(party.Field3) : 0;
            
            model.Address         = (Address)party.Address;

            model.BookingPartyID = party.BookingPartyID != null ? Convert.ToInt32(party.BookingPartyID) : 0;

            return model;
        }
    }
}