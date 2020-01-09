using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Backoffice.Filters
{
    public class GlobalAdminsFilter:ActionFilterAttribute
    {
        /// <summary>
        /// The Action filter limits the access to actions that are only for members who have a DREAMS subscription
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //2015-10-06
            //Ivan S.
            //68768 
            //Added the filter to limit the access to certain actions that are only for Global Adminstrators
            
            //Validates if the user is accesing the ManageResources Action from the ResourcesController
            if (!(Identity.Current.CustomerID == 1))
            {
                //If the user is not one of the 2 authorized ones, we redirect to the dashboard
                filterContext.Result = new RedirectToRouteResult("default", new RouteValueDictionary(new { controller = "Dashboard", action = "Index" }));
            }            
        }
    }
}