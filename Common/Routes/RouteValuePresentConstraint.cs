using System.Web;
using System.Web.Routing;

namespace Common.Routes
{
    public class RouteValuePresentConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext,
                          Route route, string parameterName,
                          RouteValueDictionary values,
                          RouteDirection routeDirection)
        {
            if (values.ContainsKey(parameterName))
            {
                return true;
            }
            return false;
        }
    }
}
