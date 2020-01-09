using Backoffice.Services;
using Common;
using Common.Services;
using ExigoService;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using Dapper;
using System.Web;
using System;
using System.IO;
using Exigo.Tokenization.TokenEx;

namespace Backoffice.Controllers
{
    public class AppController : BaseController
    {
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
            var cachetime = (Identity.Current.CustomerID == id ? 5 : 60);
            var response = ExigoDAL.Images().GetCustomerAvatarResponse(id, type, cache);
            if (cache)
            {
                Response.Cache.SetCacheability(HttpCacheability.Public);
                Response.Cache.SetLastModified(response.ModifiedDate);
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(cachetime));
            }
            // Return the image
            return File(response.Bytes, response.FileType, response.FileName);
        }

        [Route("~/profiles/rt/avatar/{id:int}/{type?}/{cache?}")]
        public FileResult RealtimeAvatar(int id, AvatarType type = AvatarType.Default, bool cache = true)
        {
            cache = (Identity.Current.CustomerID == id ? false : cache);
            var response = ExigoDAL.Images().GetCustomerAvatarResponse(id, type, cache);
            
            // Return the image
            return File(response.Bytes, response.FileType, response.FileName);
        }
        #endregion

        #region Globalization
        public ActionResult SetLanguagePreference(int id)
        {
            var language = GlobalUtilities.GetLanguage(id, CurrentMarket);

            ExigoDAL.SetCustomerPreferredLanguage(Identity.Current.CustomerID, language.LanguageID);

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
            return Json(ExigoDAL.IsLoginNameAvailable(LoginName, Identity.Current.CustomerID), JsonRequestBehavior.AllowGet);
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
            else return RedirectToAction("Index", "Dashboard");
        }

        [Route("~/StopDebug")]
        public ActionResult StopDebug(string goTo = null)
        {
            GlobalUtilities.DeleteDebugCookie();
            if (Request.IsAjaxRequest()) return Json(true);
            else if (Request.UrlReferrer != null) return Redirect(Request.UrlReferrer.AbsoluteUri);
            else if (goTo != null) return Redirect(goTo);
            else return RedirectToAction("Index", "Dashboard");
        }

        #endregion

        #region Enrollment Redirect
        public ActionResult ReplicatedSiteRedirect(int customerID = 0)
        {
            if (customerID == 0)
            {
                customerID = Identity.Current.CustomerID;
            }
            // Get Default url, if no web alias can be found
            var url = GlobalSettings.ReplicatedSites.FormattedBaseUrl.FormatWith(GlobalSettings.ReplicatedSites.DefaultWebAlias);

            var customerSite = ExigoDAL.GetCustomerSite(customerID);
            if (customerSite != null)
            {
                url = GlobalSettings.ReplicatedSites.FormattedBaseUrl.FormatWith(customerSite.WebAlias);
            }

            return Redirect(url);
        }
        public ActionResult EnrollmentRedirect()
        {
            var url = GlobalSettings.ReplicatedSites.EnrollmentUrl;

            var customerSite = ExigoDAL.GetCustomerSite(Identity.Current.CustomerID);
            if (customerSite != null)
            {
                url = GlobalSettings.ReplicatedSites.GetEnrollmentUrl(customerSite.WebAlias);
            }

            return Redirect(url);
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
