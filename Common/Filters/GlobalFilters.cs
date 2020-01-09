using System.Web.Mvc;

namespace Common.Filters
{
    public static class GlobalFilters
    {
        /// <summary>
        /// Exigo-specific API credentials and configurations
        /// </summary>
        public static void Register(GlobalFilterCollection filters)
        {
            filters.Add(new HandleExigoErrorAttribute());
            filters.Add(new ValidateAntiForgeryTokenOnPostAttribute());
        }
    }
}