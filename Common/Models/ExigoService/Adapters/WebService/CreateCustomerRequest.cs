using ExigoService;
using System;

namespace Common.Api.ExigoWebService
{
    public partial class CreateCustomerRequest
    {
        public CreateCustomerRequest() { }
        public CreateCustomerRequest(Customer customer)
        {
            CustomerType        = customer.CustomerTypeID;
            CustomerStatus      = customer.CustomerStatusID;
            LanguageID          = customer.LanguageID;
            EntryDate           = customer.CreatedDate;
            BirthDate           = customer.BirthDate;

            FirstName           = customer.FirstName;
            MiddleName          = customer.MiddleName;
            LastName            = customer.LastName;
            Email               = customer.Email;
            Phone               = customer.PrimaryPhone;
            Phone2              = customer.SecondaryPhone;
            MobilePhone         = customer.MobilePhone;
            Fax                 = customer.Fax;
            Company             = customer.Company;


            if (customer.MainAddress != null)
            {
                MainAddress1 = customer.MainAddress.Address1;
                MainAddress2 = customer.MainAddress.Address2;
                MainCity     = customer.MainAddress.City;
                MainState    = customer.MainAddress.State;
                MainZip      = customer.MainAddress.Zip;
                MainCountry  = customer.MainAddress.Country;
            }

            if (customer.MailingAddress != null)
            {
                MailAddress1 = customer.MailingAddress.Address1;
                MailAddress2 = customer.MailingAddress.Address2;
                MailCity     = customer.MailingAddress.City;
                MailState    = customer.MailingAddress.State;
                MailZip      = customer.MailingAddress.Zip;
                MailCountry  = customer.MailingAddress.Country;
            }

            if (customer.OtherAddress != null)
            {
                OtherAddress1 = customer.OtherAddress.Address1;
                OtherAddress2 = customer.OtherAddress.Address2;
                OtherCity     = customer.OtherAddress.City;
                OtherState    = customer.OtherAddress.State;
                OtherZip      = customer.OtherAddress.Zip;
                OtherCountry  = customer.OtherAddress.Country;
            }

            TaxID         = customer.TaxID;
            PayableToName = customer.PayableToName;
            PayableType   = ExigoDAL.GetPayableType(customer.PayableTypeID);

            LoginName = customer.LoginName;
            LoginPassword = customer.Password;           

            Field1  = customer.Field1;
            Field2  = customer.Field2;
            Field3  = customer.Field3;
            Field4  = customer.Field4;
            Field5  = customer.Field5;
            Field6  = customer.Field6;
            Field7  = customer.Field7;
            Field8  = customer.Field8;
            Field9  = customer.Field9;
            Field10 = customer.Field10;
            Field11 = customer.Field11;
            Field12 = customer.Field12;
            Field13 = customer.Field13;
            Field14 = customer.Field14;
            Field15 = customer.Field15;

            Date1 = customer.Date1;
            Date2 = customer.Date2;
            Date3 = customer.Date3;
            Date4 = customer.Date4;
            Date5 = customer.Date5;
        }
        public CreateCustomerRequest(Guest guest, ShippingAddress shippingAddress, int defaultWarehouseID, int enrollerID, int partyID)
        {
            CustomerType       = CustomerTypes.RetailCustomer;
            CustomerStatus     = CustomerStatuses.Active;
            DefaultWarehouseID = defaultWarehouseID;
            EntryDate          = DateTime.Now;

            FirstName          = guest.FirstName;
            LastName           = guest.LastName;
            Email              = guest.Email;
            Phone              = guest.Phone;
            Phone2             = guest.WorkPhone;
            MobilePhone        = guest.CellPhone;


            MainAddress1       = shippingAddress.Address1;
            MainAddress2       = shippingAddress.Address2;
            MainCity           = shippingAddress.City;
            MainState          = shippingAddress.State;
            MainZip            = shippingAddress.Zip;
            MainCountry        = shippingAddress.Country;

            InsertEnrollerTree = true;
            EnrollerID         = enrollerID;

            Notes              = "Customer created from Guest account #{0} and Party #{1}".FormatWith(guest.GuestID, partyID);
        }
    }
}