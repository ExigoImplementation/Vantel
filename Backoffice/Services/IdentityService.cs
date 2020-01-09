using Backoffice.Models;
using Common;
using Common.Providers;
using Common.Services;
using ExigoService;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;
using Dapper;
using Common.Api.ExigoWebService;
using Serilog;

namespace Backoffice.Services
{
    public class IdentityService
    {
        IIdentityAuthenticationProvider authProvider = new SqlIdentityAuthenticationProvider();

        public IdentityService() { }
        public IdentityService(IIdentityAuthenticationProvider provider)
        {
            authProvider = provider;
        }

        public LoginResponse SignIn(string loginname, string password)
        {
            var response = new LoginResponse();
            int customerID = 0;
            try
            {
                // Authenticate the customer
                customerID = authProvider.AuthenticateCustomer(loginname, password);
                if (customerID == 0)
                {
                    response.Fail("Unable to authenticate");
                    return response;
                }

                // Get the customer 

                var identity = GetIdentity(customerID);
                if (identity == null)
                {
                    response.Fail("Customer not found");
                    return response;
                }

                // Get the redirect URL (for silent logins) or create the forms ticket
                response.RedirectUrl = GetSilentLoginRedirect(identity);

                if (response.RedirectUrl.IsEmpty())
                CreateFormsAuthenticationTicket(customerID);

                // Mark the response as successful
                response.Success();
            }

            catch (Exception ex)
            {
                response.Fail(ex.Message);
            }
            // 3/09/17 #85791 Brian Bruneau Using customer ID, validate that no crucial customer info is missing (ex. Main Country)
            KeyValuePair<bool, List<string>> identityValidationResponse = Common.Utilities.Identity.IdentityCheck(customerID);

            // 3/09/17 #85791 Brian Bruneau if the customer is not validated, display toastr message and do not authorize login.
            if (!identityValidationResponse.Key)
            {
                response.Fail("Your profile is missing the following: <br> " + "<ol> <li>" + string.Join(" </li><li> ", identityValidationResponse.Value.ToArray()) + "</li></ol>" + "</br> <b>Please contact your administrator for assistance.</b>");
                FormsAuthentication.SignOut();
            }

            return response;
        }
        public LoginResponse SignIn(int customerid)
        {
            var response = new LoginResponse();

            try
            {
                // Authenticate the customer
                var customerID = authProvider.AuthenticateCustomer(customerid);
                if (customerID == 0)
                {
                    response.Fail("Unable to authenticate");
                    return response;
                }

                // Get the customer
                var identity = GetIdentity(customerID);
                if (identity == null)
                {
                    response.Fail("Customer not found");
                    return response;
                }

                // Get the redirect URL (for silent logins) or create the forms ticket
                response.RedirectUrl = GetSilentLoginRedirect(identity);
                if (response.RedirectUrl.IsEmpty()) CreateFormsAuthenticationTicket(customerID);

                // Mark the response as successful
                response.Success();
            }
            catch (Exception ex)
            {
                response.Fail(ex.Message);
            }

            return response;
        }
        public LoginResponse SignIn(string silentLoginToken)
        {
            var response = new LoginResponse();

            try
            {
                // Decrypt the token
                var token = Security.Decrypt(silentLoginToken);

                // Split the value and get the values

                // Return the expiration status of the token and the sign in response
                if (token.ExpirationDate < DateTime.Now)
                {
                    response.Fail("Token expired");
                    return response;
                }

                // Sign the customer in with their customer ID
                response = SignIn((int)token.CustomerID);

                // Mark the response as successful
                response.Success();
            }
            catch (Exception ex)
            {
                response.Fail(ex.Message);
            }

            return response;
        }

        public LoginResponse AdminSilentLogin(string token)
        {
            var response = new LoginResponse();

            try
            {
                // Decrypt the token
                var IV = GlobalSettings.EncryptionKeys.SilentLogins.IV;
                var key = GlobalSettings.EncryptionKeys.SilentLogins.Key;
                var decryptedToken = Security.AESDecrypt(token, key, IV);

                // Split the value and get the values
                var splitToken = decryptedToken.Split('|');
                var customerID = Convert.ToInt32(splitToken[0]);
                var tokenExpirationDate = Convert.ToDateTime(splitToken[1]);

                // Return the expiration status of the token and the sign in response
                //if (tokenExpirationDate < DateTime.Now)
                //{
                //    response.Fail("Token expired");
                //    return response;
                //}

                // Sign the customer in with their customer ID
                response = SignIn(customerID);

                // Mark the response as successful
                response.Success();
            }
            catch (Exception ex)
            {
                response.Fail(ex.Message);
            }

            return response;
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public void RefreshIdentity()
        {
            CreateFormsAuthenticationTicket(Identity.Current.CustomerID);
        }

        public UserIdentity GetIdentity(int customerID)
        {
            dynamic identity = null;
            using (var context = ExigoDAL.Sql())
            {
                try
                {
                    identity = context.Query<UserIdentity>(@"
                        SELECT  
                            c.CustomerID
                            , c.FirstName
                            , c.LastName
                            , c.Company
                            , c.LoginName                       
                            , c.CustomerTypeID
                            , c.CustomerStatusID
                            , c.LanguageID
                            , c.DefaultWarehouseID
                            , c.CurrencyCode
                            , c.MainCountry AS 'Country'

                        FROM Customers c

                        WHERE c.CustomerID = @customerid
                        AND c.CanLogin = 1 -- Ensure that the Customer is allowed to Log in
                    ", new
                    {
                        customerid = customerID
                    }).FirstOrDefault();

                    // Use the web service as a backup
                    if (identity == null)
                    {
                        var customer = ExigoDAL.WebService().GetCustomers(new GetCustomersRequest()
                        {
                            CustomerID = customerID
                        }).Customers.FirstOrDefault();

                        if (customer != null)
                        {
                            identity = new UserIdentity()
                            {
                                CustomerID = customer.CustomerID,
                                FirstName = customer.FirstName,
                                LastName = customer.LastName,
                                Company = customer.Company,
                                LoginName = customer.LoginName,
                                CustomerTypeID = customer.CustomerType,
                                CustomerStatusID = customer.CustomerStatus,
                                LanguageID = customer.LanguageID,
                                DefaultWarehouseID = customer.DefaultWarehouseID = (customer.DefaultWarehouseID > 0) ? customer.DefaultWarehouseID : Warehouses.Default,
                                CurrencyCode = customer.CurrencyCode,
                                Country = customer.MainCountry
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error Getting Identity: {Message}", ex.Message);
                    return null;
                }
            }

            return identity;
        }
        public string GetSilentLoginRedirect(UserIdentity identity)
        {
            if (identity.CustomerTypeID != CustomerTypes.Distributor)
            {
                var token = Security.Encrypt(new
                {
                    CustomerID = identity.CustomerID,
                    ExpirationDate = DateTime.Now.AddHours(1)
                });

                return GlobalSettings.Backoffices.SilentLogins.RetailCustomerBackofficeUrl.FormatWith(GlobalSettings.ReplicatedSites.DefaultWebAlias, token);
            }

            return string.Empty;
        }
        public bool CreateFormsAuthenticationTicket(int customerID)
        {
            // If we got here, we are authorized. Let's attempt to get the identity.
            var identity = GetIdentity(customerID);
            if (identity == null) return false;

            // Ensure any defaults that the customer must have
            Task.Run(() =>
            {
                EnsureIdentityDefaults(customerID);
            });

            // Create the ticket
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                identity.CustomerID.ToString(),
                DateTime.Now,
                DateTime.Now.AddMinutes(GlobalSettings.Backoffices.SessionTimeout),
                false,
                identity.SerializeProperties());


            // Encrypt the ticket
            string encTicket = FormsAuthentication.Encrypt(ticket);


            // Create the cookie.
            HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                cookie.HttpOnly = true;

                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                cookie.Value = encTicket;
                HttpContext.Current.Response.Cookies.Set(cookie);
            }


            // Add the customer ID to the items in case we need this in the same request later on.
            // We need this because we don't have access to the Identity.Current in this same request later on.
            HttpContext.Current.Items.Add("CustomerID", customerID);


            return true;
        }

        public void EnsureIdentityDefaults(int customerID)
        {
            try
            {
                ExigoDAL.EnsureCalendar(customerID);
            }
            catch { }
        }
    }
}
