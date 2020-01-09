using ExigoService;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace Common
{
    public static partial class GlobalUtilities
    {
        /// <summary>
        /// Sets the CultureCode of the site based on the current market.
        /// </summary>
        public static void SetCurrentCulture(string cultureCode)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureCode);
        }

        /// <summary>
        /// Sets the CurrentUICulture of the site based on the user's language preferences.
        /// </summary>
        public static void SetCurrentUICulture(string cultureCode)
        {
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureCode);
        }

        /// <summary>
        /// Get the selected country code 
        /// </summary>
        /// <remarks>Defaults to country of the default market</remarks>
        /// <returns></returns>
        public static string GetSelectedCountryCode(HttpContextBase context, string countryCode = null)
        {
            // get market that matches country code (or default)
            var defaultMarket = GlobalUtilities.GetMarket(countryCode);
            // get the cookie (generates using default value if not available)
            var value = GlobalUtilities.GetCookie(
                context,
                cookieName: GlobalSettings.Globalization.CountryCookieName,
                defaultValue: defaultMarket.MainCountry,
                defaultExpiration: DateTime.Now.AddYears(1),
                httpOnly: false // allow access via JavaScript
                );
            // return the value
            return value;
        }

        /// <summary>
        /// Sets the selected country code 
        /// </summary>
        /// <returns></returns>
        public static string SetSelectedCountryCode(HttpContextBase context, string countryCode)
        {
            // get the market for the selected country
            var market = GlobalUtilities.GetMarket(countryCode);
            // get the current cookie value (generates if it doesn't exist)
            var value = GetSelectedCountryCode(context, market.MainCountry);
            // if existing value is different, set with new value
            if (!value.Equals_IgnoreCase(market.MainCountry))
            {
                value = market.MainCountry;
                GlobalUtilities.SetCookie(
                    context,
                    name: GlobalSettings.Globalization.CountryCookieName,
                    value: value);

            }
            // return the value
            return value;
        }

        /// <summary>
        /// Get the selected country code 
        /// </summary>
        /// <returns></returns>
        public static string GetSelectedCountryCode(string countryCode = "US")
        {
            var cookie = HttpContext.Current.Request.Cookies[GlobalSettings.Globalization.CountryCookieName];

            if (countryCode != "US")
            {
                cookie = new HttpCookie(GlobalSettings.Globalization.CountryCookieName);
                cookie.Value = countryCode;
                cookie.HttpOnly = false;
                HttpContext.Current.Response.Cookies.Add(cookie);

                return countryCode;
            }

            if (cookie != null && !cookie.Value.IsEmpty())
            {
                return cookie.Value;
            }
            else
            {
                cookie = new HttpCookie(GlobalSettings.Globalization.CountryCookieName);
                cookie.Value = countryCode;
                cookie.HttpOnly = false;
                HttpContext.Current.Response.Cookies.Add(cookie);

                return cookie.Value;
            }
        }

        /// <summary>
        /// Sets the selected country code 
        /// </summary>
        /// <returns></returns>
        public static string SetSelectedCountryCode(string countryCode)
        {
            var cookie = HttpContext.Current.Request.Cookies[GlobalSettings.Globalization.CountryCookieName];

            if (cookie != null && !cookie.Value.IsEmpty())
            {
                cookie.HttpOnly = false;
                cookie.Value = countryCode;
                HttpContext.Current.Response.Cookies.Add(cookie);

                return cookie.Value;
            }
            else
            {
                cookie = new HttpCookie(GlobalSettings.Globalization.CountryCookieName)
                {
                    Value = countryCode,
                    HttpOnly = false
                };
                HttpContext.Current.Response.Cookies.Add(cookie);

                return cookie.Value;
            }
        }

        /// <summary>
        /// Gets the configuration, based on the country cookie
        /// </summary>
        public static Market GetCurrentMarket(HttpContextBase context)
        {
            var countryCode = GlobalUtilities.GetSelectedCountryCode(context);

            return GetMarket(countryCode);
        }

        /// <summary>
        /// Gets the first market that contains the requested country.
        /// <para>Defaults to 'IsDefault' market, and then First() market if no matching markets are found.</para>
        /// </summary>
        /// <param name="countryCode">2 digit country code. Not case sensitive</param>
        /// <returns></returns>
        public static Market GetMarket(string countryCode)
        {
            // get the market that matches the country code
            return GlobalSettings.Markets.AvailableMarkets
                .FirstOrDefault(c => c.Countries.Any(co => co.Equals_IgnoreCase(countryCode)))
                // if no market, get the default market
                ?? GlobalSettings.Markets.AvailableMarkets
                .FirstOrDefault(c => c.IsDefault)
                // if no default, get the first market
                ?? GlobalSettings.Markets.AvailableMarkets
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the configuration for the market name provided.
        /// </summary>
        public static IMarketConfiguration GetMarketConfiguration(MarketName marketName)
        {
            return GlobalSettings.Markets.AvailableMarkets.Where(c => c.Name == marketName).FirstOrDefault().GetConfiguration();
        }

        /// <summary>
        /// Get the correct Culture Code to use for displaying currencies in the correct format. 
        /// Used on pages that show Commissions and Orders
        /// </summary>
        /// <param name="currencyCode">Currency Code that the format needs to be in.</param>
        /// <param name="countryCode">Country code in case conditional logic needs to apply. Ex. German Euro (1.234,56€) vs. Italy Euro (€1.234,56)</param>
        /// <returns>Culture Code for formatting Currencies</returns>
        public static string GetCultureCodeFormatBasedOnCurrency(string currencyCode, string countryCode)
        {
            var cultureCode = "en-US";
            

            switch (currencyCode.ToLower())
            {
                case CurrencyCodes.Euro:
                    switch (countryCode)
                    {
                        case "IT":
                        cultureCode = "it-IT";
                            break;
                        case "DE":
                            cultureCode = "de-DE";
                            break;
                        case "ES":
                            cultureCode = "es-ES";
                            break;
                        default:
                            break;
                    }
                    break;
                case CurrencyCodes.PoundsUK:
                    cultureCode = "en-GB";
                    break;
                case CurrencyCodes.DollarsUS:
                default:
                    break;
            }

            return cultureCode;
        }

    }
}