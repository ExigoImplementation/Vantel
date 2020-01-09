namespace Common.Api.ExigoWebService
{
    public partial class GetCustomerSiteResponse
    {
        public static explicit operator ExigoService.CustomerSite(GetCustomerSiteResponse site)
        {
            var model = new ExigoService.CustomerSite();
            if (site == null) return model;

            model.CustomerID     = site.CustomerID;
            model.WebAlias       = site.WebAlias;
            model.FirstName      = site.FirstName;
            model.LastName       = site.LastName;
            model.Company        = site.Company;

            model.Email          = site.Email;
            model.PrimaryPhone   = site.Phone;
            model.SecondaryPhone = site.Phone2;
            model.Fax            = site.Fax;

            model.Address        = new ExigoService.Address
            {
                AddressType      = ExigoService.AddressType.Other,
                Address1         = site.Address1,
                Address2         = site.Address2,
                City             = site.City,
                State            = site.State,
                Zip              = site.Zip,
                Country          = site.Country
            };

            model.Notes1         = site.Notes1;
            model.Notes2         = site.Notes2;
            model.Notes3         = site.Notes3;
            model.Notes4         = site.Notes4;

            return model;
        }
    }
}