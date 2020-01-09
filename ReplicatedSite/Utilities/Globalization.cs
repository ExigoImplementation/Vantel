using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using ExigoService;
namespace ReplicatedSite
{
    public partial class Utilities
    {
        /// <summary>
        /// Gets the market the website is currently using.
        /// </summary>
        /// <returns>The Market object representing the current market.</returns>
        public static Market GetCurrentMarket()
        {
            // Get the user's country to see which market we are in
            var country = Common.GlobalUtilities.GetSelectedCountryCode();

            if (country.IsNullOrEmpty())
            {
                country = GlobalSettings.Markets.AvailableMarkets.Where(c => c.IsDefault == true).FirstOrDefault().Countries.FirstOrDefault();
            }

            // If the country cookie in null or empty then create it
            var countryCookie = Common.GlobalUtilities.SetSelectedCountryCode(country);

            var market = GlobalSettings.Markets.AvailableMarkets.Where(c => c.Countries.Contains(country)).FirstOrDefault();

            // If we didn't find a market for the user's country, get the first default market
            if (market == null) market = GlobalSettings.Markets.AvailableMarkets.Where(c => c.IsDefault == true).FirstOrDefault();

            // If we didn't find a default market, get the first market we find
            if (market == null) market = GlobalSettings.Markets.AvailableMarkets.FirstOrDefault();

            // Return the market
            return market;
        }
    }
}