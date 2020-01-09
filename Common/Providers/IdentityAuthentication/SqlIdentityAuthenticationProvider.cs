using Common.Helpers;
using ExigoService;

namespace Common.Providers
{
    public class SqlIdentityAuthenticationProvider : IIdentityAuthenticationProvider
    {
        public int AuthenticateCustomer(string loginname, string password)
        {
            var command = new SqlHelper();
            var customerID = command.GetField("AuthenticateCustomer {0}, {1}", loginname, password);

            // If the SQL authentication failed, we want to make sure we try the Webservice in case the person was not just created and the Sync db has not caught up yet
            if (customerID == null)
            {
                try
                {
                    var wsAuthReponse = ExigoDAL.WebService().AuthenticateCustomer(new Api.ExigoWebService.AuthenticateCustomerRequest { LoginName = loginname, Password = password });
                    if (wsAuthReponse.Result.Status == Api.ExigoWebService.ResultStatus.Success)
                    {
                        return wsAuthReponse.CustomerID;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch { return 0; }
            }
            else
            {
                return (int)customerID;
            }
        }
        public int AuthenticateCustomer(int customerid)
        {
            var command = new SqlHelper();
            var customerID = command.GetField("select CustomerID from Customers where CustomerID = {0}", customerid);

            if (customerID == null)
            {
                // If the SQL Customer call failed, we want to make sure we try the Webservice in case the person was not just created and the Sync db has not caught up yet
                try
                {
                    var getCustomerResponse = ExigoDAL.WebService().GetCustomers(new Api.ExigoWebService.GetCustomersRequest { CustomerID = customerid });
                    if (getCustomerResponse.Result.Status == Api.ExigoWebService.ResultStatus.Success && getCustomerResponse.Customers.Length > 0)
                    {
                        return getCustomerResponse.Customers[0].CustomerID;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch { return 0; }
            }
            else
            {
                return (int)customerID;
            }
        }
    }
}