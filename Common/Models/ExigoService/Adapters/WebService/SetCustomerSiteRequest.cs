using ExigoService;

namespace Common.Api.ExigoWebService
{
    public partial class SetCustomerSiteRequest
    {
        public SetCustomerSiteRequest() { }
        public SetCustomerSiteRequest(Customer customer)
        {
            CustomerID = customer.CustomerID;
            WebAlias = customer.LoginName;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Company = customer.Company;

            Email = customer.Email;
            Phone = customer.PrimaryPhone;
            Phone2 = customer.SecondaryPhone;
            Fax = customer.Fax;

            if (customer.MainAddress != null)
            {
                Address1 = customer.MainAddress.Address1;
                Address2 = customer.MainAddress.Address2;
                City = customer.MainAddress.City;
                State = customer.MainAddress.State;
                Zip = customer.MainAddress.Zip;
                Country = customer.MainAddress.Country;
            }
        }
        public SetCustomerSiteRequest(CustomerSite request)
        {
            CustomerID   = request.CustomerID;
            WebAlias     = request.WebAlias;
            FirstName    = request.FirstName;
            LastName     = request.LastName;
            Company      = request.Company;

            Email        = request.Email;
            Phone        = request.PrimaryPhone;
            Phone2       = request.SecondaryPhone;
            Fax          = request.Fax;

            if (request.Address != null)
            {
                Address1 = request.Address.Address1;
                Address2 = request.Address.Address2;
                City     = request.Address.City;
                State    = request.Address.State;
                Zip      = request.Address.Zip;
                Country  = request.Address.Country;
            }

            Notes1       = request.Notes1;
            Notes2       = request.Notes2;
            Notes3       = request.Notes3;
            Notes4       = request.Notes4;
        }
    }
}