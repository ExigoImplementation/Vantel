using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Common;

namespace Backoffice.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class IgnoreContentManagerAdminFilterAttribute : AuthorizeAttribute
    {

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class ContentManagerAdminsFilter : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.ActionDescriptor.IsDefined(typeof(IgnoreContentManagerAdminFilterAttribute), true)
                && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(IgnoreContentManagerAdminFilterAttribute), true))
            {
                base.OnAuthorization(filterContext);
            }
        }

        public bool Response { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //Response = Identity.Current.CustomerTypeID == Common.CustomerTypes.Master;
            Response = Identity.Current.CustomerTypeID == Common.CustomerTypes.PreferredCustomer;
            return Response;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            throw new ArgumentNullException("filterContext");
        }
    }
}