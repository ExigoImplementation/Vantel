using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Api.ExigoWebService
{
    public partial class PartyAddress
    {
        public static explicit operator ExigoService.Address(PartyAddress address)
        {
            var model = new ExigoService.Address();
            if (address == null) return model;

            model.Address1 = address.Address1;
            model.Address2 = address.Address2;
            model.City = address.City;
            model.State = address.State;
            model.Zip = address.Zip;
            model.Country = address.Country;
            
            return model;
        }

        public static explicit operator PartyAddress(ExigoService.Address address)
        {
            var model = new PartyAddress();
            if (address == null) return model;

            model.Address1 = address.Address1;
            model.Address2 = address.Address2;
            model.City = address.City;
            model.State = address.State;
            model.Zip = address.Zip;
            model.Country = address.Country;

            return model;
        }
    }
}