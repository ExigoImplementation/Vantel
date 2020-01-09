using Backoffice.Filters;
using Common.Filters;
using System.Web.Mvc;

namespace Backoffice
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new BackofficeAuthorizeAttribute());
            filters.Add(new RequireSecureConnectionFilter());
            filters.Add(new HandleExigoErrorAttribute());
        }
    }
}