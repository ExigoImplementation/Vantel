using System.Security.Principal;
using Common.Providers;

namespace ReplicatedSite.Providers
{
    public interface IReplicatedSiteIdentityAuthenticationProvider : IIdentityAuthenticationProvider
    {
        /// <summary>
        /// Get an identity for the replicated site owner.
        /// </summary>
        /// <param name="webAlias">The replicated site owner's web alias.</param>
        /// <returns>The replicated site owner's identity</returns>
        ReplicatedSiteIdentity GetSiteOwnerIdentity(string webAlias);

        /// <summary>
        /// Get an identity for an existing customer signing in to the site.
        /// </summary>
        /// <param name="customerID">The customer's ID.</param>
        /// <returns>The existing customer's identity</returns>
        CustomerIdentity GetCustomerIdentity(int customerID);
    }
}
