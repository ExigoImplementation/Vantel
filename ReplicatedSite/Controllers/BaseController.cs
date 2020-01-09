using Common;
using ExigoService;
using System.Linq;
using System.Web.Mvc;

namespace ReplicatedSite.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewData["isFullscreen"] = Request.QueryString["isFullscreen"];
        }

        private bool? _isDebug { get; set; }
        public bool IsDebug {
            get {
                if (_isDebug == null)
                {
                    _isDebug = System.Configuration.ConfigurationManager.AppSettings["ReleaseMode"] == "debug";
                }
                return _isDebug.Value;
            }
        }
        public Market _currentMarket { get; set; }
        public Market CurrentMarket {
            get {
                if (_currentMarket == null)
                {
                    if (Request.IsAuthenticated && Identity.Customer != null)
                    {
                        #region User locked market (default)
                        _currentMarket = Identity.Customer.Market;
                        #endregion
                        #region User selected market
                        //_currentMarket = GlobalUtilities.GetCurrentMarket(this.HttpContext);
                        #endregion
                    }
                    else
                    {
                        #region Owner based default market (default)
                        var country = GlobalUtilities.GetSelectedCountryCode(this.HttpContext, Identity.Owner.Market.MainCountry);
                        _currentMarket = GlobalUtilities.GetMarket(country);
                        #endregion
                        #region User selected market
                        //_currentMarket = GlobalUtilities.GetCurrentMarket(this.HttpContext);
                        #endregion
                    }
                }
                return _currentMarket;
            }
        }
        public Language _currentLanguage { get; set; }
        public Language CurrentLanguage {
            get {
                if (_currentLanguage == null)
                {
                    _currentLanguage = GlobalUtilities.GetSelectedLanguage(this.HttpContext, null, this.CurrentMarket);
                }
                return _currentLanguage;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(_isDebug != null) { _isDebug = null; }
                if(_currentMarket != null) { _currentMarket = null; }
                if(_currentLanguage != null) { _currentLanguage = null; }
            }
            base.Dispose(disposing);
        }
    }
}