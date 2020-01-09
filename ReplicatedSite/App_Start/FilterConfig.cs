using ReplicatedSite.Filters;
using System.Web.Mvc;

namespace ReplicatedSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {           
            filters.Add(new RequireSecureConnectionFilter());           
        }      
    }
}