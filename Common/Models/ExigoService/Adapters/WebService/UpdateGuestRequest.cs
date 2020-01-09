﻿using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Api.ExigoWebService
{
    public partial class UpdateGuestRequest
    {
        public UpdateGuestRequest() { }
        public static explicit operator UpdateGuestRequest(ExigoService.Guest guest)
        {
            var model = new UpdateGuestRequest();
            if (guest == null) return model;

            model.GuestID = guest.GuestID;

            model.FirstName = guest.FirstName;
            model.LastName = guest.LastName;
            model.Address1 = guest.Address1;
            model.Address2 = guest.Address2;
            model.City = guest.City;
            model.State = guest.State;
            model.Zip = guest.Zip;
            model.Country = guest.Country;
            model.Email = guest.Email;

            // Flag to determine if a user should be able to recieve emails from Hostess. This would be for the Party Invite.
            model.Field1 = guest.AllowEmail.ToString();

            if (guest.CustomerID != 0)
            {
                model.CustomerID = guest.CustomerID;
            }


            model.Phone = guest.Phone;
            model.Phone2 = guest.WorkPhone;
            model.MobilePhone = guest.CellPhone;
            //model.Notes = guest.Notes;
            
            return model;
        }
    }
}