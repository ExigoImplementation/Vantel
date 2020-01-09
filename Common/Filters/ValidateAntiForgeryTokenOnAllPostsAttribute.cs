using System;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Common.Filters
{
    //Added by Elliott Q. on 6/3/15, written by Travis W. ||| Globally validates anti forgery token on post instead of using and onBegin function in Ajax.BeginForm
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class ValidateAntiForgeryTokenOnPostAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            // Only validate POSTs
            if (request.HttpMethod == WebRequestMethods.Http.Post)
            {
                // Bypass validation if we are allowing unvalidated posts on this method.
                if (!filterContext.ActionDescriptor.IsDefined(typeof(ExternalHttpPostAttribute), true)
                && !filterContext.ActionDescriptor.IsDefined(typeof(ExternalHttpPostAttribute), false))
                {
                    var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];

                    var cookieValue = antiForgeryCookie != null
                        ? antiForgeryCookie.Value
                        : null;

                    AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"] ?? request["__RequestVerificationToken"]);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }


    [AttributeUsage(AttributeTargets.All)]
    public class ExternalHttpPostAttribute : AuthorizeAttribute
    {
    }
}
