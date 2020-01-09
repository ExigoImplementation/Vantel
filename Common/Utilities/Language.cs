using ExigoService;
using System;
using System.Globalization;
using System.Linq;
using System.Web;


namespace Common
{
    public static partial class GlobalUtilities
    {
        public static CultureInfo GetRequestedLanguageCultureInfo(HttpRequestBase request)
        {
            var userLanguages = request.UserLanguages;


            CultureInfo ci;
            if (userLanguages != null && userLanguages.Any())
            {
                try
                {
                    ci = new CultureInfo(userLanguages[0].Trim());
                }
                catch (CultureNotFoundException)
                {
                    ci = CultureInfo.InvariantCulture;
                }
            }
            else
            {
                ci = CultureInfo.InvariantCulture;
            }

            return ci;
        }

        public static string GetRequestedCountry(HttpRequestBase request)
        {
            var culture = GetRequestedLanguageCultureInfo(request);

            var regionInfo = new RegionInfo(culture.LCID);
            return regionInfo.TwoLetterISORegionName;
        }

        public static string GetSelectedCultureCode(HttpContextBase context, string defaultCultureCode = null)
        {
            // get the cookie (generates using default value if not available)
            var value = GlobalUtilities.GetCookie(
                context,
                cookieName: GlobalSettings.Globalization.LanguageCookieName,
                defaultValue: defaultCultureCode,
                defaultExpiration: DateTime.Now.AddYears(1),
                httpOnly: false // allow access via JavaScript
                );

            return value;
        }

        public static Language GetSelectedLanguage(HttpContextBase context, string defaultCultureCode = null, Market market = null)
        {
            market = market ?? GetCurrentMarket(context);
            var cultureCode = GetSelectedCultureCode(context, defaultCultureCode);
            var language = GlobalUtilities.GetLanguage(cultureCode, market);
            return language;
        }

        /// <summary>
        /// Overload for GetLanguage(cultureCode that gets the culture code by language ID.
        /// </summary>
        /// <param name="languageID"></param>
        /// <returns></returns>
        public static Language GetLanguage(int languageID, Market market)
        {
            #region Market specific languages (default)
            var result =
                // get the requested language from the requested market
                market?.AvailableLanguages?.FirstOrDefault(l => l.LanguageID == languageID)
                // get the default language from the requested market
                ?? market?.AvailableLanguages.FirstOrDefault()
                // get the requested language from the requested market's country
                ?? GetMarket(market.MainCountry)?.AvailableLanguages?.FirstOrDefault(l => l.LanguageID == languageID)
                // get the default language from the requested market's country
                ?? GetMarket(market.MainCountry)?.AvailableLanguages.FirstOrDefault()
                // get the requested language from the default market
                ?? GetMarket(string.Empty)?.AvailableLanguages?.FirstOrDefault(l => l.LanguageID == languageID)
                // get the default language from the default market
                ?? GetMarket(string.Empty)?.AvailableLanguages.FirstOrDefault();
            #endregion
            #region Global Languages
            // requires you to set up global language list in settings.
            // remove [market] parameter when switching to this logic
            //var result =
            //    // get the matching language
            //    GlobalSettings.Languages.AvailableLanguages.FirstOrDefault(l => l.LanguageID == languageID)
            //    // get the default language
            //    ?? GlobalSettings.Languages.AvailableLanguages.FirstOrDefault();
            #endregion
            return result;
        }

        /// <summary>
        /// Gets the first language that contains the requested culture code.
        /// </summary>
        /// <param name="cultureCode">culture code of the language to match</param>
        /// <returns></returns>
        public static Language GetLanguage(string cultureCode, Market market)
        {

            #region Market specific languages (default)
            var result =
                // get the requested language from the requested market
                market?.AvailableLanguages?.FirstOrDefault(l => l.CultureCode.Equals_IgnoreCase(cultureCode))
                // get the default language from the requested market
                ?? market?.AvailableLanguages.FirstOrDefault()
                // get the requested language from the requested market's country
                ?? GetMarket(market.MainCountry)?.AvailableLanguages?.FirstOrDefault(l => l.CultureCode.Equals_IgnoreCase(cultureCode))
                // get the default language from the requested market's country
                ?? GetMarket(market.MainCountry)?.AvailableLanguages.FirstOrDefault()
                // get the requested language from the default market
                ?? GetMarket(string.Empty)?.AvailableLanguages?.FirstOrDefault(l => l.CultureCode.Equals_IgnoreCase(cultureCode))
                // get the default language from the default market
                ?? GetMarket(string.Empty)?.AvailableLanguages.FirstOrDefault();
            #endregion
            #region Global Languages
            // requires you to set up global language list in settings.
            // remove [market] parameter when switching to this logic
            //var result =
            //    // get the matching language
            //    GlobalSettings.Languages.AvailableLanguages.FirstOrDefault(l => l.CultureCode.Equals_IgnoreCase(cultureCode))
            //    // get the default language
            //    ?? GlobalSettings.Languages.AvailableLanguages.FirstOrDefault();
            #endregion
            return result;

        }

        public static Language GetLanguageByCustomerID(int customerID)
        {
            // Get the user's language preference based on their saved preference
            var customer = ExigoDAL.GetCustomer(customerID);
            var market = GlobalUtilities.GetMarket(customer.MainAddress.Country);
            var language = GlobalUtilities.GetLanguage(customer.LanguageID, market);

            // Return the language
            return language;
        }
    }
}