using Common;
using Common.Services;
using Dapper;
using Exigo.Tokenization.TokenEx;
using ExigoService;
using Newtonsoft.Json;
using ReplicatedSite.Services;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ReplicatedSite.Controllers
{
    public class AppController : BaseController
    {
        #region Global & Local Resources
        [AllowAnonymous]
        public JavaScriptResult Resource(string name = "resources", string path = "Resources")
        {
            // NOTE: The GetJavaScript method assumes you will be precompiling the project during publishing.
            // IF this is not the case, you will need to remove the isLocal parameters, as the App_GlobalResources file will not be compiled into the bin file. 
            // Locally this folder is outside of the bin folder
            // Also note that in order for the resx files to be updated correctly you must open the App_GlobalResources folder in the Common project (individual projects not necessary), select all the .resx files, right-click and choose properties, and then, in the Copy to Output Directory, choose "copy always"

            // Clean up any references to .resx - our code enters that automatically.
            if (path.Contains(".resx")) path = path.Replace(".resx", "");

            // Create our factory
            var service = new ClientResourceService();
            service.JavaScriptObjectName = name;
            service.GlobalResXFileName = path;

            // Write our javascript to the page.            
            return JavaScript(service.GetJavaScript(Request.IsLocal));
        }
        #endregion

        #region Keeping Sessions Alive
        public JsonResult KeepAlive()
        {
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Cultures
        [AllowAnonymous]
        public JavaScriptResult Culture()
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture;
            var currentUICulture = Thread.CurrentThread.CurrentUICulture;

            var adsfads = JsonConvert.SerializeObject(currentCulture.NumberFormat);

            var result = new StringBuilder();
            result.AppendFormat(@"
                CultureInfo = function (c, b, a) {{
                    this.name = c;
                    this.numberFormat = b;
                    this.dateTimeFormat = a
                }};

                CultureInfo.prototype = {{
                    _getDateTimeFormats: function () {{
                        if (!this._dateTimeFormats) {{
                            var a = this.dateTimeFormat;
                            this._dateTimeFormats = [a.MonthDayPattern, a.YearMonthPattern, a.ShortDatePattern, a.ShortTimePattern, a.LongDatePattern, a.LongTimePattern, a.FullDateTimePattern, a.RFC1123Pattern, a.SortableDateTimePattern, a.UniversalSortableDateTimePattern]
                        }}
                        return this._dateTimeFormats
                    }},
                    _getIndex: function (c, d, e) {{
                        var b = this._toUpper(c),
                            a = Array.indexOf(d, b);
                        if (a === -1) a = Array.indexOf(e, b);
                        return a
                    }},
                    _getMonthIndex: function (a) {{
                        if (!this._upperMonths) {{
                            this._upperMonths = this._toUpperArray(this.dateTimeFormat.MonthNames);
                            this._upperMonthsGenitive = this._toUpperArray(this.dateTimeFormat.MonthGenitiveNames)
                        }}
                        return this._getIndex(a, this._upperMonths, this._upperMonthsGenitive)
                    }},
                    _getAbbrMonthIndex: function (a) {{
                        if (!this._upperAbbrMonths) {{
                            this._upperAbbrMonths = this._toUpperArray(this.dateTimeFormat.AbbreviatedMonthNames);
                            this._upperAbbrMonthsGenitive = this._toUpperArray(this.dateTimeFormat.AbbreviatedMonthGenitiveNames)
                        }}
                        return this._getIndex(a, this._upperAbbrMonths, this._upperAbbrMonthsGenitive)
                    }},
                    _getDayIndex: function (a) {{
                        if (!this._upperDays) this._upperDays = this._toUpperArray(this.dateTimeFormat.DayNames);
                        return Array.indexOf(this._upperDays, this._toUpper(a))
                    }},
                    _getAbbrDayIndex: function (a) {{
                        if (!this._upperAbbrDays) this._upperAbbrDays = this._toUpperArray(this.dateTimeFormat.AbbreviatedDayNames);
                        return Array.indexOf(this._upperAbbrDays, this._toUpper(a))
                    }},
                    _toUpperArray: function (c) {{
                        var b = [];
                        for (var a = 0, d = c.length; a < d; a++) b[a] = this._toUpper(c[a]);
                        return b
                    }},
                    _toUpper: function (a) {{
                        return a.split(""\u00a0"").join("" "").toUpperCase()
                    }}
                }};
                CultureInfo._parse = function (a) {{
                    var b = a.dateTimeFormat;
                    if (b && !b.eras) b.eras = a.eras;
                    return new CultureInfo(a.name, a.numberFormat, b)
                }};


                CultureInfo.InvariantCulture = CultureInfo._parse({{
                    {0}
                }});

                CultureInfo.CurrentCulture = CultureInfo._parse({{
                    {1}
                }});

            ", GetCultureInfoJson(currentCulture), GetCultureInfoJson(currentUICulture));


            return JavaScript(result.ToString());
        }
        private string GetCultureInfoJson(CultureInfo cultureInfo)
        {
            var result = new StringBuilder();

            result.AppendFormat(@"
                ""name"": ""{0}"",
                ""numberFormat"": {1},
                ""dateTimeFormat"": {2},
                ""eras"": [1, ""A.D."", null, 0]
            ",
                cultureInfo.Name,
                JsonConvert.SerializeObject(cultureInfo.NumberFormat),
                JsonConvert.SerializeObject(cultureInfo.DateTimeFormat));

            return result.ToString();
        }
        #endregion

        #region Countries & Regions
        [OutputCache(Duration = 86400)]
        public JsonNetResult GetCountries()
        {
            var countries = ExigoDAL.GetCountries();

            return new JsonNetResult(new
            {
                success = true,
                countries = countries
            });
        }

        [OutputCache(VaryByParam = "id", Duration = 86400)]
        public JsonNetResult GetRegions(string id)
        {
            var regions = ExigoDAL.GetRegions(id);

            return new JsonNetResult(new
            {
                success = true,
                regions = regions
            });
        }
        #endregion

        #region Avatars
        [Route("~/profiles/avatar/{id:int}/{type?}/{cache?}")]
        public FileResult Avatar(int id, AvatarType type = AvatarType.Default, bool cache = true)
        {
            var response = ExigoDAL.Images().GetCustomerAvatarResponse(id, type, cache);
            if (cache)
            {
                Response.Cache.SetCacheability(HttpCacheability.Public);
                Response.Cache.SetLastModified(response.ModifiedDate);
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(60));
            }
            // Return the image
            return File(response.Bytes, response.FileType, response.FileName);
        }
        #endregion

        #region Globalization
        public ActionResult SetLanguagePreference(int id)
        {
            var language = GlobalUtilities.GetLanguage(id, CurrentMarket);

            ExigoDAL.SetCustomerPreferredLanguage(Identity.Customer.CustomerID, language.LanguageID);

            GlobalUtilities.SetCurrentUICulture(language.CultureCode);

            new IdentityService().RefreshIdentity();

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
        #endregion

        #region Validation
        public JsonResult VerifyAddress(Address address)
        {
            return Json(ExigoDAL.VerifyAddress(address), JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsLoginNameAvailable([Bind(Prefix = "Customer.LoginName")]string LoginName)
        {
            var customerID = Identity.Customer != null ? Identity.Customer.CustomerID : 0;
            return Json(ExigoDAL.IsLoginNameAvailable(LoginName, customerID), JsonRequestBehavior.AllowGet);
        }
        public JsonResult IsWebaliasAvailable([Bind(Prefix = "CustomerSite.Webalias")]string Webalias)
        {
            return Json(ExigoDAL.IsWebaliasAvailable(Webalias), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Debug

        [Route("~/StartDebug")]
        public ActionResult StartDebug(string goTo = null)
        {
            GlobalUtilities.SetDebugCookie();
            if (Request.IsAjaxRequest()) return Json(true);
            else if (Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.AbsoluteUri);
            else if (goTo != null) return Redirect(goTo);
            else return RedirectToAction("Index", "Home");
        }

        [Route("~/StopDebug")]
        public ActionResult StopDebug(string goTo = null)
        {
            GlobalUtilities.DeleteDebugCookie();
            if (Request.IsAjaxRequest()) return Json(true);
            else if (Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.AbsoluteUri);
            else if (goTo != null) return Redirect(goTo);
            else return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Item Images
        [AllowAnonymous]
        [Route("~/shopping/productimages/{imageName}")]
        public ActionResult ProductImages(string imageName)
        {
            object bytes;

            using (var context = ExigoDAL.Sql())
            {
                var query = @"SELECT TOP 1 
                                ImageData 
                              FROM 
                                ItemImages 
                              WHERE 
                                ImageName = @Name";

                bytes = context.ExecuteScalar(query, new { Name = imageName });
            }

            if (bytes == null)
                return HttpNotFound();
            else
            {
                var extension = Path.GetExtension(imageName).ToLower();
                string contentType = "image/jpeg";

                switch (extension)
                {
                    case ".gif":
                        contentType = "image/gif";
                        break;
                    case ".jpg":
                        contentType = "image/jpeg";
                        break;
                    case ".jpeg":
                        contentType = "image/png";
                        break;
                    case ".bmp":
                        contentType = "image/bmp";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                }


                Response.Cache.SetCacheability(HttpCacheability.Public);
                Response.Cache.SetLastModified(DateTime.Parse("1/1/1900"));
                Response.Cache.SetExpires(DateTime.Now.AddYears(1));

                return File((byte[])bytes, contentType);
            }
        }
        #endregion

        #region Enroller Search Modal
        public JsonNetResult GetEnrollerSearchModal()
        {
            try
            {
                var html = this.RenderPartialViewToString("_EnrollerModal", null);

                return new JsonNetResult(new
                {
                    success = true,
                    html
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }

        }
        #endregion

        #region Token Ex
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public async Task<ActionResult> TokenEx(TokenExActionRequest req)
        {
            var iframecontroller = new TokenExIFrameController(
                tokenExId: GlobalSettings.Exigo.PaymentApi.LoginName,
                clientSecret: GlobalSettings.Exigo.PaymentApi.Password,
                serverUri: Request.Url.AbsoluteUri
            );

            var res = await iframecontroller.ServerActionAsync(req);
            return Content(res.Content, res.ContentType);
        }
        #endregion

    }
}
