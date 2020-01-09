using System;
using System.Web;

namespace Common
{
    public static partial class GlobalUtilities
    {
        private static string _lastWebAliasCookieName = GlobalSettings.Globalization.CookieKey + "_LastWebAlias";


        public static string GetWebAlias(int customerID)
        {
            var webalias = GlobalSettings.ReplicatedSites.DefaultWebAlias;

            try
            {
                webalias = ExigoService.ExigoDAL.GetCustomerSite(customerID).WebAlias;
            }
            catch { }

            return webalias;
        }
        public static string GetLastWebAlias(string defaultWebAlias)
        {
            var cookie = HttpContext.Current.Request.Cookies[_lastWebAliasCookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(_lastWebAliasCookieName);
            }

            if (string.IsNullOrEmpty(cookie.Value))
            {
                return defaultWebAlias;
            }

            return cookie.Value;
        }
        public static void SetLastWebAlias(string webAlias)
        {
            var cookie = HttpContext.Current.Request.Cookies[_lastWebAliasCookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(_lastWebAliasCookieName);
            }

            cookie.Value = webAlias;
            cookie.Expires = DateTime.Now.AddYears(1);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
        public static void DeleteLastWebAlias()
        {
            var cookie = HttpContext.Current.Request.Cookies[_lastWebAliasCookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
    }
}