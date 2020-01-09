using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Common.HtmlHelpers
{
    public static class UrlCompileHelper
    {
        public static string Action(string action)
        {
            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));

            // Determine some web alias data
            var urlHelper = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current))));
            var webalias = (routeData.Values["webalias"] != null) ? routeData.Values["webalias"].ToString() : string.Empty;
            return urlHelper.Action(action, new { webalias });
        }
        public static string Action(string action, string controller)
        {
            var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current));

            // Determine some web alias data
            var urlHelper = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), RouteTable.Routes.GetRouteData(new HttpContextWrapper(HttpContext.Current))));
            var webalias = (routeData.Values["webalias"] != null) ? routeData.Values["webalias"].ToString() : string.Empty;
            return urlHelper.Action(action, controller, new { webalias });
        }
    }
}