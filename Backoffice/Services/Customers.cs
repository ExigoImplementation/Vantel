using Backoffice;
using ExigoService;
using System.Collections.Generic;

namespace Backoffice
{
    public static partial class Customers
    {
        public static Customer GetCustomer(int customerID)
        {
            UserIdentity ident = null; 
            try { ident = Identity.Current; } catch { } //null-safe for tests
            // If we are getting the report for the current user, than we will want to get it in real time
            return ExigoDAL.GetCustomer(customerID: customerID, realtime: (ident != null && (customerID == ident.CustomerID)));
        }

        public static IEnumerable<Address> GetCustomerAddresses(int customerID)
        {
            UserIdentity ident = null;
            try { ident = Identity.Current; } catch { } //null-safe for tests
            return ExigoDAL.GetCustomerAddresses(customerID, (ident != null ? (customerID == ident.CustomerID) : false));
        }
    }
}