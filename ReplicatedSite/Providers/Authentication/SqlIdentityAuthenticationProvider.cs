using ExigoService;
using System.Linq;
using Common;
using Dapper;
using Common.Api.ExigoWebService;

namespace ReplicatedSite.Providers
{
    public class SqlIdentityAuthenticationProvider : Common.Providers.SqlIdentityAuthenticationProvider, IReplicatedSiteIdentityAuthenticationProvider
    {
        public ReplicatedSiteIdentity GetSiteOwnerIdentity(string webAlias)
        {
            var replicatedsiteidentity = new ReplicatedSiteIdentity();

            using (var context = ExigoDAL.Sql())
            {
                var customerID = context.Query<int>(@"
                    SELECT cs.CustomerID FROM CustomerSites cs WHERE cs.WebAlias = @webalias
                ", new
                {
                    webalias = webAlias
                }).FirstOrDefault();

                // Logic to handle default web alias missing from database, which would cause an infinite redirect loop
                if (customerID == 0 && webAlias.Equals(GlobalSettings.ReplicatedSites.DefaultWebAlias, System.StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new System.Exception("Default user missing. Web Alias: {0}".FormatWith(GlobalSettings.ReplicatedSites.DefaultWebAlias));
                }

                if (customerID > 0)
                {
                    replicatedsiteidentity = context.Query<ReplicatedSiteIdentity>(@"
                    ; WITH cte_SocialNetworks AS (
	                    SELECT 
		                    CustomerID
		                    , [1] as FacebookURL
		                    , [2] as GooglePlusURL
		                    , [3] AS TwitterURL
		                    , [4] AS BlogURL
		                    , [5] AS LinkedInURL
		                    , [6] AS MySpaceURL
		                    , [7] AS YouTubeURL
		                    , [8] AS PinterestURL
		                    , [9] AS InstagramURL
	                    FROM 
	                    (SELECT 
			                    SocialNetworkID
			                , Customerid
			                , Url 
	                     FROM CustomerSocialNetworks
	                     WHERE CustomerID = @customerid) AS SourceTable
	                    PIVOT
	                    (
	                    MAX(url)
	                    for SocialNetworkID in ([1],[2],[3],[4],[5],[6],[7],[8],[9])
	                    ) as PivotTable
                    )
                    SELECT
                        c.CustomerID
                        , c.CustomerTypeID
                        , c.CustomerStatusID
	                    , c.CreatedDate
                        , ISNULL(c.RankID, 0) AS 'HighestAchievedRankID'    
	                    , ISNULL(c.DefaultWarehouseID, @defaultwarehouseid) AS 'WarehouseID'    
                        , cs.WebAlias
                        , FirstName = CASE WHEN cs.FirstName = '' THEN c.FirstName ELSE cs.FirstName END
                        , LastName = CASE WHEN cs.LastName = '' THEN c.LastName ELSE cs.LastName END
                        , cs.Company
                        , cs.Email
                        , cs.Phone
                        , cs.Phone2
                        , cs.Fax
                        , cs.Address1
                        , cs.Address2
                        , cs.City
                        , cs.State
                        , cs.Zip
                        , cs.Country
                        , cs.Notes1
                        , cs.Notes2
                        , cs.Notes3
                        , cs.Notes4
                        , ISNULL(s.FacebookUrl, '')	  AS 'FacebookUrl'
	                    , ISNULL(s.GooglePlusUrl, '') AS 'GooglePlusUrl'
	                    , ISNULL(s.TwitterUrl, '')	  AS 'TwitterUrl'
	                    , ISNULL(s.BlogUrl, '')		  AS 'BlogUrl'
	                    , ISNULL(s.LinkedInUrl, '')	  AS 'LinkedInUrl' 
	                    , ISNULL(s.MyspaceUrl, '')	  AS 'MyspaceUrl'
	                    , ISNULL(s.YouTubeUrl, '')	  AS 'YouTubeUrl'
	                    , ISNULL(s.PinterestUrl, '')  AS 'PinterestUrl'
	                    , ISNULL(s.InstagramUrl, '')  AS 'InstagramUrl'

                    FROM CustomerSites cs
                        LEFT JOIN Customers c
                            ON c.CustomerID = cs.CustomerID 
	                    LEFT JOIN cte_SocialNetworks s
		                    ON c.CustomerID = s.CustomerID 
								      
                    WHERE cs.CustomerID = @customerid
                ", new
                    {
                        defaultwarehouseid = Warehouses.Default,
                        customerid = customerID
                    }).FirstOrDefault();
                }
            }

            // Webservice fail safe that will handle any recent enrollees if the sql hasn't caught up yet
            if (replicatedsiteidentity.CustomerID > 0)
            {
                return replicatedsiteidentity;
            }
            else
            {
                var context = ExigoDAL.WebService();
                var customerSite = new GetCustomerSiteResponse();

                // Check to see if customer site for the provided webalias exists. Failure is thrown if not sso do this in a try/catch
                try
                {
                    customerSite = context.GetCustomerSite(new GetCustomerSiteRequest()
                    {
                        WebAlias = webAlias
                    });
                }
                catch (System.Exception)
                {
                    return null;
                }

                var customer = new Customer();

                if (customerSite.CustomerID > 0)
                {
                    customer = (Customer)context.GetCustomers(new GetCustomersRequest()
                    {
                        CustomerID = customerSite.CustomerID
                    }).Customers.FirstOrDefault();

                    var socialNetworksResponse = context.GetCustomerSocialNetworks(new GetCustomerSocialNetworksRequest()
                    {
                        CustomerID = customerSite.CustomerID
                    });

                    if (socialNetworksResponse.CustomerSocialNetwork.Count() > 0)
                    {
                        foreach (var network in socialNetworksResponse.CustomerSocialNetwork)
                        {
                            switch (network.SocialNetworkID)
                            {
                                case SocialNetworkTypes.Facebook:
                                    replicatedsiteidentity.FacebookUrl = network.Url;
                                    continue;
                                case SocialNetworkTypes.GooglePlus:
                                    replicatedsiteidentity.GooglePlusUrl = network.Url;
                                    continue;
                                case SocialNetworkTypes.Twitter:
                                    replicatedsiteidentity.TwitterUrl = network.Url;
                                    continue;
                                case SocialNetworkTypes.Blog:
                                    replicatedsiteidentity.BlogUrl = network.Url;
                                    continue;
                                case SocialNetworkTypes.LinkedIn:
                                    replicatedsiteidentity.LinkedInUrl = network.Url;
                                    continue;
                                case SocialNetworkTypes.MySpace:
                                    replicatedsiteidentity.MySpaceUrl = network.Url;
                                    continue;
                                case SocialNetworkTypes.YouTube:
                                    replicatedsiteidentity.YouTubeUrl = network.Url;
                                    continue;
                                case SocialNetworkTypes.Pinterest:
                                    replicatedsiteidentity.PinterestUrl = network.Url;
                                    continue;
                                case SocialNetworkTypes.Instagram:
                                    replicatedsiteidentity.InstagramUrl = network.Url;
                                    continue;
                                default:
                                    continue;
                            }
                        }
                    }

                    // customer
                    replicatedsiteidentity.CustomerID = customer.CustomerID;
                    replicatedsiteidentity.CustomerTypeID = customer.CustomerTypeID;
                    replicatedsiteidentity.CustomerStatusID = customer.CustomerStatusID;
                    replicatedsiteidentity.HighestAchievedRankID = customer.RankID;
                    replicatedsiteidentity.CreatedDate = customer.CreatedDate;
                    replicatedsiteidentity.WarehouseID = (customer.DefaultWarehouseID != 0) ? customer.DefaultWarehouseID : Warehouses.Default;

                    // customer site
                    replicatedsiteidentity.WebAlias = customerSite.WebAlias;
                    replicatedsiteidentity.FirstName = customerSite.FirstName;
                    replicatedsiteidentity.LastName = customerSite.LastName;
                    replicatedsiteidentity.Company = customerSite.Company;
                    replicatedsiteidentity.Email = customerSite.Email;
                    replicatedsiteidentity.Phone = customerSite.Phone;
                    replicatedsiteidentity.Phone2 = customerSite.Phone2;
                    replicatedsiteidentity.Fax = customerSite.Fax;
                    replicatedsiteidentity.Address1 = customerSite.Address1;
                    replicatedsiteidentity.Address2 = customerSite.Address2;
                    replicatedsiteidentity.City = customerSite.City;
                    replicatedsiteidentity.State = customerSite.State;
                    replicatedsiteidentity.Zip = customerSite.Zip;
                    replicatedsiteidentity.Country = customerSite.Country;
                    replicatedsiteidentity.Notes1 = customerSite.Notes1;
                    replicatedsiteidentity.Notes2 = customerSite.Notes2;
                    replicatedsiteidentity.Notes3 = customerSite.Notes3;
                    replicatedsiteidentity.Notes4 = customerSite.Notes4;

                    return replicatedsiteidentity;
                }

                return null;
            }
        }
        public CustomerIdentity GetCustomerIdentity(int customerID)
        {
            var customer = new CustomerIdentity();

            try
            {
                using (var context = ExigoDAL.Sql())
                {
                    customer = context.Query<CustomerIdentity>(@"
                    SELECT
                        c.CustomerID                        
                        , c.FirstName
                        , c.LastName
                        , c.Company
                        , c.LoginName
                        , c.CustomerTypeID
                        , c.CustomerStatusID
                        , c.LanguageID
                        , DefaultWarehouseID = COALESCE(c.DefaultWarehouseID, @defaultwarehouseid)
                        , c.CurrencyCode
                        , Country = c.MainCountry
                        
                    FROM Customers c
                    WHERE c.CustomerID = @customerid
                        AND c.CanLogin = 1 -- Ensure that the Customer is allowed to Log in
                ", new
                    {
                        customerid = customerID,
                        defaultwarehouseid = Warehouses.Default
                    }).FirstOrDefault();
                }

                // Web Service failsafe if the Customer has yet to appear in the Sync SQL
                if (customer != null)
                {
                    return customer;
                }
                else
                {
                    var apiCustomer = (Customer)ExigoDAL.WebService().GetCustomers(new GetCustomersRequest()
                    {
                        CustomerID = customerID
                    }).Customers.FirstOrDefault();

                    if (apiCustomer != null && apiCustomer.CustomerID != 0)
                    {
                        var customerIdentity = new CustomerIdentity()
                        {
                            CustomerID         = apiCustomer.CustomerID,                            
                            FirstName          = apiCustomer.FirstName,
                            LastName           = apiCustomer.LastName,
                            Company            = apiCustomer.Company,
                            LoginName          = apiCustomer.LoginName,
                            CustomerTypeID     = apiCustomer.CustomerTypeID,
                            CustomerStatusID   = apiCustomer.CustomerStatusID,
                            LanguageID         = apiCustomer.LanguageID,
                            DefaultWarehouseID = (apiCustomer.DefaultWarehouseID != 0) ? apiCustomer.DefaultWarehouseID : Warehouses.Default,
                            CurrencyCode       = apiCustomer.CurrencyCode,
                            Country            = apiCustomer.MainAddress.Country
                        };

                        return customerIdentity;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
