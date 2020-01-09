using Common;
using Common.Api.ExigoWebService;
using System;
using System.Linq;
using Dapper;
using Serilog;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static CustomerSite GetCustomerSite(int customerID)
        {
            try
            {
                dynamic site;
                var customerSite = new CustomerSite();

                //Get the raw data
                using (var context = ExigoDAL.Sql())
                {
                    site = context.Query(@"
                            SELECT
	                            cs.CustomerID
	                            ,cs.WebAlias
	                            ,cs.FirstName
	                            ,cs.LastName
	                            ,cs.Company
	                            ,cs.Email
	                            ,cs.Phone
	                            ,cs.Phone2                                
	                            ,cs.Address1
	                            ,cs.Address2
	                            ,cs.City
	                            ,cs.State
	                            ,cs.Zip
	                            ,cs.Country
	                            ,cs.Fax
	                            ,cs.Notes1
	                            ,cs.Notes2
	                            ,cs.Notes3
	                            ,cs.Notes4

                            FROM
	                            CustomerSites cs

                            WHERE
	                            cs.CustomerID = @customerid                      
                            ", new
                            {
                                customerid = customerID
                            }).FirstOrDefault();

                    if (site == null) { return customerSite; }

                    // Map the data to the model (done this way to populate the Address object correctly)
                    customerSite.CustomerID = site.CustomerID;
                    customerSite.WebAlias = site.WebAlias;
                    customerSite.FirstName = site.FirstName;
                    customerSite.LastName = site.LastName;
                    customerSite.Company = site.Company;
                    customerSite.Email = site.Email;
                    customerSite.PrimaryPhone = site.Phone;
                    customerSite.SecondaryPhone = site.Phone2;
                    customerSite.Address.AddressType = AddressType.Other;
                    customerSite.Address.Address1 = site.Address1;
                    customerSite.Address.Address2 = site.Address2;
                    customerSite.Address.City = site.City;
                    customerSite.Address.State = site.State;
                    customerSite.Address.Zip = site.Zip;
                    customerSite.Address.Country = site.Country;
                    customerSite.Fax = site.Fax;
                    customerSite.Notes1 = site.Notes1;
                    customerSite.Notes2 = site.Notes2;
                    customerSite.Notes3 = site.Notes3;
                    customerSite.Notes4 = site.Notes4;
                }

                return customerSite;
            }
            catch (Exception exception)
            {
                if (exception.Message == "CustomerSite not found\n") return new CustomerSite();
                else throw exception;
            }
        }
        public static CustomerSite GetCustomerSiteRealTime(int customerID)
        {

            var site = new CustomerSite();

            // The GetCustomerSite method currently throws an exception if no customer site record exists for the customer
            // We will set the site for this customer in the Catch and get the site again if this is the case
            try
            {
                var apiCustomerSite = ExigoDAL.WebService().GetCustomerSite(new GetCustomerSiteRequest { CustomerID = customerID });

                site = (CustomerSite)apiCustomerSite;
            }
            catch (Exception)
            {
                // Do Nothing
            }

            return site;
        }
        public static CustomerSite UpdateCustomerSite(CustomerSite request)
        {
            // There is no way to update the customer site with the web service - it only allows you to set all the fields.
            // Essentially, it's an all-or-none approach.
            // This method will let us update only the fields we want to update.

            // If the customer site passed is null, or we don't have the CustomerID set, stop here.
            if (request == null || request.CustomerID == 0) return request;


            // First, get the existing customer's site info
            var customerSite = GetCustomerSite(request.CustomerID);
            if (customerSite != null)
            {
                // Determine if the web alias has changed between the request and the existing data.
                // If it isn't available, set the requested web alias to null so we don't attempt to update it.
                if (request.WebAlias.IsNullOrEmpty())
                {
                    request.WebAlias = customerSite.WebAlias;
                }
                else if (request.WebAlias.ToUpper() != customerSite.WebAlias.ToUpper() && !IsWebAliasAvailable(request.CustomerID, request.WebAlias))
                {
                    request.WebAlias = null;
                }



                // Reflect each property and populate it if the requested value is not null.
                var customerSiteType = customerSite.GetType();
                var address = customerSite.Address;
                foreach (var property in customerSiteType.GetProperties())
                {
                    if (property.CanWrite && property.GetValue(request) != GlobalUtilities.GetDefault(property.PropertyType))
                    {
                        property.SetValue(customerSite, property.GetValue(request));
                    }
                }


                // Reflect the address separately
                customerSite.Address = address;
                var addressType = customerSite.Address.GetType();
                foreach (var property in addressType.GetProperties())
                {
                    if (property.CanWrite && property.GetValue(request.Address) != GlobalUtilities.GetDefault(property.PropertyType))
                    {
                        property.SetValue(customerSite.Address, property.GetValue(request.Address));
                    }
                }
            }
            else
            {
                customerSite = request;
                if (customerSite.WebAlias.IsNullOrEmpty())
                {
                    customerSite.WebAlias = customerSite.CustomerID.ToString();
                }
                if (customerSite.WebAlias.IsNotNullOrEmpty() && !IsWebAliasAvailable(customerSite.CustomerID, customerSite.WebAlias))
                {
                    return customerSite;
                }
            }

            // Update the data
            SetCustomerSite(customerSite);

            // Return the modified request we used to update the data
            return customerSite;
        }
        public static CustomerSite SetCustomerSite(CustomerSite request)
        {
            ExigoDAL.WebService().SetCustomerSite(new SetCustomerSiteRequest(request));
            return request;
        }

        public static bool IsWebAliasAvailable(int customerID, string webalias)
        {
            try
            {
                // Get the current webalias to see if it matches what we passed. If so, it's still valid.
                var currentWebAlias = ExigoDAL.GetCustomerSite(customerID).WebAlias;
                if (webalias.Equals(currentWebAlias, StringComparison.InvariantCultureIgnoreCase)) return true;

                // Validate the web alias
                // cannot use SQL due to delay in update to replicated database
                var customerSite = ExigoDAL.WebService().GetCustomerSite(new GetCustomerSiteRequest()
                {
                    WebAlias = webalias
                });

                return (customerSite == null);
            }
            catch (Exception ex)
            {
                // the GetCustomerSite throws an exception if no matching web alias is found
                // return the web alias is available in this case so return true
                Log.Error(ex, "Error Getting Replicated Site: {Message}", ex.Message);
                return true;
            }
        }
        public static bool IsWebaliasAvailable(string webalias)
        {
            var webaliasAvailable = false;

            // must use try catch as API returns an exception if no customer site with the provided webalias is found    
            try
            {
                // cannot use SQL due to delay in update to replicated database
                var customerSite = ExigoDAL.WebService().GetCustomerSite(new GetCustomerSiteRequest()
                {
                    WebAlias = webalias
                });

                if (customerSite == null)
                {
                    webaliasAvailable = true;
                };
            }
            catch (Exception ex)
            {
                webaliasAvailable = true;
                Log.Error(ex, "Error Getting Replicated Site: {Message}", ex.Message);
            }


            return webaliasAvailable;
        }
    }
}