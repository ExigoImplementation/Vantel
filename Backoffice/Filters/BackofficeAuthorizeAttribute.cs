using System;
using System.Web.Mvc;

namespace Backoffice.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class BackofficeAuthorizeAttribute : AuthorizeAttribute
    {
         public override void OnAuthorization(AuthorizationContext filterContext)
         {
             if (filterContext.HttpContext.Request.IsAuthenticated)
             {
                 var identity = filterContext.HttpContext.User.Identity as UserIdentity;
                 if (identity == null)
                 {
                     base.OnAuthorization(filterContext);
                     return;
                 }
             }
             else
             {
                 base.OnAuthorization(filterContext);
             }
         }
    }
}