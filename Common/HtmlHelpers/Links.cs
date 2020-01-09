using System;
using System.Web.Mvc;
using Common.Services;

namespace Common.HtmlHelpers
{
    public static class LinksHtmlHelpers
    {
        public static MvcHtmlString SilentLoginToken(this UrlHelper helper, int customerID)
        {
            var token = Security.Encrypt(new
            {
                CustomerID = customerID,
                ExpirationDate = DateTime.Now.AddHours(1)
            });

            return new MvcHtmlString(token);
        }
        /*
        public static MvcHtmlString SampleSilentLogin(this UrlHelper helper)
        {
            var token = SilentLoginToken(helper);
            var url = "http://www.exigo.com/silentlogin";
            var separator = (!url.Contains("?")) ? "?" : "&";
            return new MvcHtmlString(url + separator + "token=" + SilentLoginToken(helper));
        }*/
    }
}