namespace Common.Providers
{
    public interface IIdentityAuthenticationProvider
    {
        /// <summary>
        /// Authenticates the customer with their login name and password.
        /// </summary>
        /// <param name="loginname">The customer's login name</param>
        /// <param name="password">The customer's password</param>
        /// <returns>Whether or not the customer was successfully authenticated. 0 = false</returns>
        int AuthenticateCustomer(string loginname, string password);

        /// <summary>
        /// Authenticates the customer with their customer ID. Intended for use with silent logins.
        /// </summary>
        /// <param name="customerid">The customer's customer ID</param>
        /// <returns>Whether or not the customer was successfully authenticated. 0 = false</returns>
        int AuthenticateCustomer(int customerid);
    }
}
