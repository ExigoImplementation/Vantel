using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Common.Providers
{
    public abstract class BaseLogicProvider : ILogicProvider
    {
        #region Properties
        public Controller Controller { get; set; }

        public string ControllerName
        {
            get { return Controller.RouteData.Values["controller"].ToString(); }
        }

        public UrlHelper UrlHelper
        {
            get
            {
                if (_urlHelper == null)
                {
                    _urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                }
                return _urlHelper;
            }
        }
        private UrlHelper _urlHelper;
        #endregion

        #region Constructors
        public BaseLogicProvider() {  }
        public BaseLogicProvider(Controller controller)
        {
            Controller  = controller;
        }
        #endregion

        #region Logic
        public virtual bool IsAuthenticated()
        {
            return HttpContext.Current.Request.IsAuthenticated;
        }
        #endregion

        #region Public Methods
        public abstract CheckLogicResult CheckLogic();
        public virtual ActionResult GetNextAction()
        {
            return CheckLogic().NextAction;
        }
        #endregion

        #region Helpers
        public ActionResult RedirectToAction(string action)
        {
            return RedirectToAction(action, ControllerName, null);
        }
        public ActionResult RedirectToAction(string action, string controller)
        {
            return RedirectToAction(action, controller, null);
        }
        public ActionResult RedirectToAction(string action, RouteValueDictionary routeValues)
        {
            return RedirectToAction(action, ControllerName, routeValues);
        }
        public ActionResult RedirectToAction(string action, string controller, RouteValueDictionary routeValues)
        {
            if (string.IsNullOrEmpty(controller)) controller = ControllerName;

            var values = new RouteValueDictionary
            {
                { "controller", controller },
                { "action", action }
            };

            if(routeValues != null)
            {
                foreach (var key in routeValues.Keys)
                {
                    object value = null;
                    if(routeValues.TryGetValue(key, out value))
                    {
                        values.Add(key, value);
                    }
                }
            }

            return new RedirectToRouteResult(values);
        }
        #endregion
    }
}
