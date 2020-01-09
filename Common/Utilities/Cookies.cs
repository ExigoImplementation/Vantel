using System;
using System.Linq;
using System.Web;

namespace Common
{
    public static partial class GlobalUtilities
    {
        public static bool GetHasTableChangedCookie(HttpContextBase context, string tableName)
        {
            var cookie = context.Request.Cookies[SQLTableCookie.BaseCookieName + tableName];

            // if no cookie, return false
            if (cookie == null) return false;

            long datetimeTicks = 0;
            var isValid = long.TryParse(cookie.Value, out datetimeTicks);

            // if not valid ticks, return false
            if (!isValid) { return false; }

            // if cookie was not created within the last 5 minutes (server time) return false
            if (datetimeTicks < DateTime.Now.AddMinutes(-5).Ticks) { return false; }

            return true;
        }

        public static void SetHasTableChangedCookie(HttpContextBase context, string tableName)
        {
            SetCookie(context, SQLTableCookie.BaseCookieName + tableName, DateTime.Now.Ticks.ToString(), DateTime.Now.AddDays(1));
        }
        public static string GetCookie(HttpContextBase context, string cookieName, object defaultValue = null, DateTime? defaultExpiration = null, bool? httpOnly = null)
        {
            var cookie = (context.Response.Cookies.AllKeys.Contains(cookieName) ? context.Response.Cookies[cookieName] : null)
                ?? context.Request.Cookies[cookieName]
                ?? GenerateCookie(context, cookieName, defaultValue, defaultExpiration, httpOnly);

            return cookie.Value;
        }
        public static void SetCookie(HttpContextBase context, string name, string value)
        {
            SetCookie(context, name, value, DateTime.Now.AddMonths(1));
        }
        public static void SetCookie(HttpContextBase context, string name, string value, DateTime expiration, bool? httpOnly = null)
        {
            var cookie = (context.Response.Cookies.AllKeys.Contains(name) ? context.Response.Cookies[name] : null)
                ?? context.Request.Cookies[name]
                ?? GenerateCookie(context, name, value, expiration, httpOnly);

            cookie.Value = value;
            cookie.Expires = expiration;

            if (context.Response.Cookies.AllKeys.Contains(name))
            {
                context.Response.Cookies.Remove(name);
            }
            context.Response.Cookies.Add(cookie);
        }
        public static void DeleteCookie(HttpContextBase context, string name)
        {
            DeleteCookie(context.ApplicationInstance.Context, name);
        }
        public static void DeleteCookie(HttpContext context, string name)
        {
            var cookie = (context.Response.Cookies.AllKeys.Contains(name) ? context.Response.Cookies[name] : null)
                ?? context.Request.Cookies[name];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);

                if (context.Response.Cookies.AllKeys.Contains(name))
                {
                    context.Response.Cookies.Remove(name);
                }
                context.Response.Cookies.Add(cookie);
            }
        }

        private static HttpCookie GenerateCookie(HttpContextBase context, string name, object defaultValue = null, DateTime? expiration = null, bool? httpOnly = null)
        {
            var cookie = new HttpCookie(name);

            cookie.Value = defaultValue == null ? null : defaultValue.ToString();
            if (expiration != null)
            {
                cookie.Expires = (DateTime)expiration;
            }
            cookie.HttpOnly = httpOnly ?? true;

            // all requests that are not localhost should be secure
            cookie.Secure = !context.Request.IsLocal;

            if (context.Response.Cookies.AllKeys.Contains(name))
            {
                context.Response.Cookies.Remove(name);
            }
            context.Response.Cookies.Add(cookie);

            return cookie;
        }
    }
}