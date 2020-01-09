namespace Common.Api.ExigoWebService
{
    public partial class CustomerResponse
    {
        public static explicit operator ExigoService.Customer(CustomerResponse customer)
        {
            var model                   = new ExigoService.Customer();
            if (customer == null) return model;

            model.CustomerID            = customer.CustomerID;
            model.FirstName             = customer.FirstName;
            model.MiddleName            = customer.MiddleName;
            model.LastName              = customer.LastName;
            model.Company               = customer.Company;

            model.CustomerTypeID        = customer.CustomerType;
            model.CustomerStatusID      = customer.CustomerStatus;
            model.DefaultWarehouseID    = customer.DefaultWarehouseID;
            model.LanguageID            = customer.LanguageID;
            model.CreatedDate           = customer.CreatedDate;
            model.BirthDate             = customer.BirthDate;
            model.IsOptedIn             = customer.IsSubscribedToBroadcasts;
            model.CurrencyCode          = customer.CurrencyCode;

            model.Email                 = customer.Email;
            model.PrimaryPhone          = customer.Phone;
            model.SecondaryPhone        = customer.Phone2;
            model.MobilePhone           = customer.MobilePhone;
            model.Fax                   = customer.Fax;

            model.MainAddress           = new ExigoService.Address {
                 AddressType            = ExigoService.AddressType.Main,
                 Address1               = customer.MainAddress1,
                 Address2               = customer.MainAddress2,
                 City                   = customer.MainCity,
                 State                  = customer.MainState,
                 Zip                    = customer.MainZip,
                 Country                = customer.MainCountry
            };
            model.MailingAddress        = new ExigoService.Address {
                AddressType             = ExigoService.AddressType.Other,
                Address1                = customer.MailAddress1,
                Address2                = customer.MailAddress2,
                City                    = customer.MailCity,
                State                   = customer.MailState,
                Zip                     = customer.MailZip,
                Country                 = customer.MailCountry
            };
            model.OtherAddress          = new ExigoService.Address {
                AddressType             = ExigoService.AddressType.Other,
                Address1                = customer.OtherAddress1,
                Address2                = customer.OtherAddress2,
                City                    = customer.OtherCity,
                State                   = customer.OtherState,
                Zip                     = customer.OtherZip,
                Country                 = customer.OtherCountry
            };

            model.LoginName             = customer.LoginName;

            model.EnrollerID            = customer.EnrollerID;
            model.SponsorID             = customer.SponsorID;
            model.HighestAchievedRank   = new ExigoService.Rank() { RankID = customer.RankID, RankDescription = customer.RankID.ToString() };

            model.Field1                = customer.Field1;
            model.Field2                = customer.Field2;
            model.Field3                = customer.Field3;
            model.Field4                = customer.Field4;
            model.Field5                = customer.Field5;
            model.Field6                = customer.Field6;
            model.Field7                = customer.Field7;
            model.Field8                = customer.Field8;
            model.Field9                = customer.Field9;
            model.Field10               = customer.Field10;
            model.Field11               = customer.Field11;
            model.Field12               = customer.Field12;
            model.Field13               = customer.Field13;
            model.Field14               = customer.Field14;
            model.Field15               = customer.Field15;

            model.Date1                 = customer.Date1;
            model.Date2                 = customer.Date2;
            model.Date3                 = customer.Date3;
            model.Date4                 = customer.Date4;
            model.Date5                 = customer.Date5;

            return model;
        }
    }
}