using Backoffice.Models;
using Backoffice.Providers;
using Backoffice.Services;
using Backoffice.ViewModels;
using Common;
using Common.Api.ExigoWebService;
using Common.Providers;
using Common.Services;
using ExigoService;
using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using Dapper;
using System.Collections.Generic;
using Serilog;

namespace Backoffice.Controllers
{
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        #region Properties
        public string PropertyBagName = GlobalSettings.Globalization.CookieKey + "GateKeeper";

        public GateKeeperPropertyBag PropertyBag
        {
            get
            {
                if (_propertyBag == null)
                {
                    _propertyBag = ExigoDAL.PropertyBags.Get<GateKeeperPropertyBag>(PropertyBagName + "PropertyBag");
                }
                return _propertyBag;
            }
        }
        private GateKeeperPropertyBag _propertyBag;

        public ILogicProvider LogicProvider
        {
            get
            {
                if (_logicProvider == null)
                {
                    _logicProvider = new GatekeeperLogicProvider(this, PropertyBag);
                }
                return _logicProvider;
            }
        }
        private ILogicProvider _logicProvider;
        #endregion

        #region Signing in
        [Route("~/login")]
        public ActionResult Login()
        {
            var model = new LoginViewModel();

            if (GlobalSettings.Exigo.Api.CompanyKey == "exigodemo")
            {
                model.LoginName = "www";
                model.Password = "testpsswd";
            }

            return View(model);
        }

        [HttpPost]
        [Route("~/login")]
        public JsonNetResult Login(LoginViewModel model)
        {
            var service = new IdentityService();
            var response = service.SignIn(model.LoginName, model.Password);

            return new JsonNetResult(response);
        }

        [Route("~/silentlogin")]
        public ActionResult SilentLogin(string token)
        {
            var service = new IdentityService();
            var response = service.SignIn(token);

            if (response.Status)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [Route("~/adminlogin")]
        public ActionResult AdminLogin(string token)
        {
            var service = new IdentityService();
            var response = service.AdminSilentLogin(token);

            if (response.Status)
            {
                if (response.RedirectUrl.IsNullOrEmpty())
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return Redirect(response.RedirectUrl);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        #endregion

        #region Signing Out
        [Route("~/logout")]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        #endregion        

        #region Gatekeeper        
        public ActionResult AccountStatus()
        {
            return View();
        }

        public ActionResult Continue()
        {
            return LogicProvider.GetNextAction();
        }
        #endregion

        #region Forgot Password
        [AllowAnonymous]
        [HttpGet]
        [Route("forgotpassword")]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("forgotpassword")]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            //Search if email exists
            var statuses = new int[] { CustomerStatuses.Active };

            try
            {
                var customers = new List<Customer>();

                using (var sqlContext = ExigoDAL.Sql())
                {
                    customers = sqlContext.Query<Customer>(@"
                    select CustomerID, Email, CanLogin
                    from Customers 
                    where CustomerStatusID in @statuses
                        and Email = @email
                        and CanLogin = 1 -- Ensure CanLogin = true before letting them proceed
                    ", new
                    {
                        statuses,
                        email = model.Email
                    }).ToList();
                }

                if (customers == null || customers.Count() == 0)
                {
                    throw new Exception("<strong>{0}</strong>".FormatWith(Resources.Common.EmailNotFound));
                }


                if (customers.Count() > 1)
                {
                    throw new Exception("<strong>{0}</strong>".FormatWith(Resources.Common.TooManyEmails));
                }


                //Generate Link to reset password
                var customer = customers.FirstOrDefault();
                var token = Security.Encrypt(new { CustomerID = customer.CustomerID, Date = DateTime.Now });
                var url = Url.Action("ResetPassword", "Authentication", new { token = token }, HttpContext.Request.Url.Scheme);
                var emailBody = Resources.Common.ForgotPasswordEmailBody.FormatWith(url);

                try
                {
                    ExigoDAL.SendEmail(new ExigoService.SendEmailRequest
                    {
                        To = new[] { model.Email },
                        From = GlobalSettings.Emails.NoReplyEmail,
                        ReplyTo = new[] { GlobalSettings.Emails.NoReplyEmail },
                        Subject = Resources.Common.ForgotPasswordEmailSubject,
                        Body = emailBody
                    });
                }
                catch (Exception e)
                {
                    throw e;
                }

                return Json(new { success = true });
            }
            catch (Exception e)
            {

                return Json(new { success = false, message = e.Message });
            }
        }
        
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string token)
        {
            //Created Model
            var model = new ResetPasswordViewModel();


            try
            {
                var decryptedToken = Security.Decrypt(token);
                var customerID = decryptedToken.CustomerID;
                var date = Convert.ToDateTime(decryptedToken.Date);
                var customer = Customers.GetCustomer((int)customerID);
                var dateExpiration = date.AddMinutes(30);

                if (DateTime.Now > dateExpiration)
                {
                    model.IsExpiredLink = true;
                }

                model.CustomerID = customer.CustomerID;
                model.CustomerType = customer.CustomerTypeID;

                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Invalid Password Reset Token Used: {Message}", ex.Message);
                return RedirectToAction("Login", "Authentication", new { error = "invalidtoken" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                ExigoDAL.UpdateCustomer(new UpdateCustomerRequest()
                {
                    CustomerID = model.CustomerID,
                    LoginPassword = model.Password
                });


                var urlHelper = new UrlHelper(Request.RequestContext);
                var url = GlobalSettings.Company.BaseBackofficeUrl + "/login";

                return new JsonNetResult(new
                {
                    success = true,
                    url
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
    }
}
