using Common;
using Common.Api.ExigoWebService;
using Common.Services;
using ExigoService;
using ReplicatedSite.Models;
using ReplicatedSite.Services;
using ReplicatedSite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using Common.Filters;
using Dapper;
using Serilog;

namespace ReplicatedSite.Controllers
{
    [Authorize]
    [RoutePrefix("{webalias}/account")]
    public class AccountController : BaseController
    {
        #region Properties

        public string ShoppingCartName = "ReplicatedSiteShopping";

        public ShoppingCartItemsPropertyBag ShoppingCart
        {
            get
            {
                if (_shoppingCart == null)
                {
                    _shoppingCart = ExigoDAL.PropertyBags.Get<ShoppingCartItemsPropertyBag>(ShoppingCartName + "Cart");
                }
                return _shoppingCart;
            }
        }
        private ShoppingCartItemsPropertyBag _shoppingCart;

        #endregion

        #region Overview
        [Route("settings")]
        public ActionResult Index(string ReturnUrl)
        {
            ReturnUrl = string.Empty;
            var model = new AccountSummaryViewModel();
            var customer = Customers.GetCustomer(Identity.Customer.CustomerID);

            model.Customer = customer;

            return View(model);
        }

        [HttpParamAction]
        public JsonNetResult UpdateEmailAddress(Customer customer)
        {
            ExigoDAL.UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = Identity.Customer.CustomerID,
                Email = customer.Email
            });

            ExigoDAL.SendEmailVerification(Identity.Customer.CustomerID, customer.Email);

            var html = string.Format("{0}", customer.Email);

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateEmailAddress",
                html = html
            });
        }

        [HttpParamAction]
        public JsonNetResult UpdateNotifications(Customer customer)
        {
            var html = string.Empty;

            try
            {
                var token = Security.Encrypt(new
                {
                    CustomerID = Identity.Customer.CustomerID,
                    Email = customer.Email
                });

                if (customer.IsOptedIn)
                {
                    ExigoDAL.SendEmailVerification(Identity.Customer.CustomerID, customer.Email);
                    html = string.Format("{0}", Resources.Common.PendingOptedInStatus);
                }
                else
                {
                    ExigoDAL.OptOutCustomer(Identity.Customer.CustomerID);
                    html = string.Format("{0}", Resources.Common.OptedOutStatus);
                }
            }
            catch
            {
                
            }

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateNotifications",
                html = html
            });
        }

        [HttpParamAction]
        public JsonNetResult UpdateName(Customer customer)
        {
            ExigoDAL.UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = Identity.Customer.CustomerID,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            });

            var html = string.Format("{0} {1}, {3}# {2}", customer.FirstName, customer.LastName, Identity.Customer.CustomerID, Resources.Common.ID);

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateName",
                html = html
            });
        }

        [HttpParamAction]
        public JsonNetResult UpdateLoginName(Customer customer)
        {
            ExigoDAL.UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = Identity.Customer.CustomerID,
                LoginName = customer.LoginName
            });

            var html = string.Format("{0}", customer.LoginName);

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateLoginName",
                html = html
            });
        }
        public JsonResult IsValidLoginName(string loginname)
        {
            var isValid = ExigoDAL.IsLoginNameAvailable(loginname, Identity.Customer.CustomerID);

            if (isValid) return Json(true, JsonRequestBehavior.AllowGet);
            else return Json(string.Format(Resources.Common.LoginNameNotAvailable, loginname), JsonRequestBehavior.AllowGet);
        }

        [HttpParamAction]
        public JsonNetResult UpdatePassword(string password)
        {
            ExigoDAL.UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = Identity.Customer.CustomerID,
                LoginPassword = password
            });

            var html = "********";

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdatePassword",
                html = html
            });
        }

        [HttpParamAction]
        public JsonNetResult UpdateLanguagePreference(Customer customer)
        {
            var language = GlobalUtilities.GetLanguage(customer.LanguageID, CurrentMarket);

            ExigoDAL.SetCustomerPreferredLanguage(Identity.Customer.CustomerID, language.LanguageID);

            GlobalUtilities.SetCurrentUICulture(language.CultureCode);

            var html = language.LanguageDescription;

            // Refresh the identity in case the country changed
            new IdentityService().RefreshIdentity();

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateLanguagePreference",
                html = html
            });
        }

        [HttpParamAction]
        public JsonNetResult UpdatePhoneNumbers(Customer customer)
        {
            ExigoDAL.UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = Identity.Customer.CustomerID,
                Phone = customer.PrimaryPhone,
                Phone2 = customer.SecondaryPhone
            });

            var html = string.Format(@"
                " + Resources.Common.Primary + @": <strong>{0}</strong><br />
                " + Resources.Common.Secondary + @": <strong>{1}</strong>
                ", customer.PrimaryPhone, customer.SecondaryPhone);

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdatePhoneNumbers",
                html = html
            });
        }
        #endregion

        #region Creating an account
        [AllowAnonymous]
        [HttpGet]
        [Route("register")]
        public ActionResult Register(string ReturnUrl)
        {
            var model = new AccountRegistrationViewModel();
            model.ReturnUrl = ReturnUrl;

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public ActionResult Register(string ReturnUrl, AccountRegistrationViewModel model)
        {
            try
            {

                // Get the current market 
                var configuration = CurrentMarket.Configuration.Orders;

                // Create the request
                var request = new CreateCustomerRequest();

                request.InsertEnrollerTree = true;
                request.FirstName          = model.Customer.FirstName;
                request.LastName           = model.Customer.LastName;
                request.Email              = model.Customer.Email;
                request.CanLogin           = true;
                request.LoginName          = model.Customer.LoginName;
                request.LoginPassword      = model.Password;
                request.CustomerType       = CustomerTypes.RetailCustomer;
                request.CustomerStatus     = CustomerStatuses.Active;
                request.EntryDate          = DateTime.Now.ToCST();
                request.DefaultWarehouseID = configuration.WarehouseID;
                request.CurrencyCode       = configuration.CurrencyCode;
                request.LanguageID         = GlobalUtilities.GetSelectedLanguage(this.HttpContext).LanguageID;
                request.EnrollerID         = Identity.Owner.CustomerID;
                request.MainCountry        = CurrentMarket.MainCountry;

                // Create the customer
                var response = ExigoDAL.WebService().CreateCustomer(request);

                // Opt in the customer if they choose to receive emails
                if (model.Customer.IsOptedIn)
                {
                    ExigoDAL.SendEmailVerification(response.CustomerID, request.Email);
                }

                // Sign in the customer
                var service = new IdentityService();
                service.SignIn(model.Customer.LoginName, model.Password);

                // Redirect the customer to the appropriate place, depending on if they got here from shopping
                if (ReturnUrl.IsNotEmpty())
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("index", "account", new { webalias = Identity.Owner.WebAlias });
                }
            }
            catch (Exception ex)
            {
                var newModel = new AccountRegistrationViewModel();
                newModel.ErrorMessage = ex.Message;
                model.ReturnUrl = ReturnUrl;

                return View(newModel);
            }
        }
        #endregion

        #region Addresses
        [Route("addresses")]
        public ActionResult AddressList()
        {
            var model = Customers.GetCustomerAddresses(Identity.Customer.CustomerID).Where(c => c.IsComplete).ToList();

            return View(model);
        }

        [Route("addresses/edit/{type:alpha}")]
        public ActionResult ManageAddress(AddressType type)
        {
            var model = Customers.GetCustomerAddresses(Identity.Customer.CustomerID).Where(c => c.AddressType == type).FirstOrDefault();

            return View("ManageAddress", model);
        }

        [Route("addresses/new")]
        public ActionResult AddAddress()
        {
            var model = new Address();
            model.AddressType = AddressType.New;
            model.Country = CurrentMarket.MainCountry;

            return View("ManageAddress", model);
        }

        [Route("deleteaddress")]
        public ActionResult DeleteAddress(AddressType type)
        {
            ExigoDAL.DeleteCustomerAddress(Identity.Customer.CustomerID, type);

            return RedirectToAction("AddressList");
        }

        [Route("setprimaryaddress")]
        public ActionResult SetPrimaryAddress(AddressType type)
        {
            ExigoDAL.SetCustomerPrimaryAddress(Identity.Customer.CustomerID, type);

            return RedirectToAction("AddressList");
        }

        [Route("saveaddress")]
        [HttpPost]
        public ActionResult SaveAddress(Address address, bool? makePrimary)
        {
            address = ExigoDAL.SetCustomerAddressOnFile(Identity.Customer.CustomerID, address);
        
            if (makePrimary != null && ((bool)makePrimary) == true)
            {
                ExigoDAL.SetCustomerPrimaryAddress(Identity.Customer.CustomerID, address.AddressType);
            }

            return RedirectToAction("AddressList");
        }
        #endregion

        #region Payment Methods
        [Route("paymentmethods")]
        public ActionResult PaymentMethodList()
        {
            var model = ExigoDAL.GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
            {
                CustomerID = Identity.Customer.CustomerID,
                ExcludeIncompleteMethods = true
            });

            return View(model);
        }

        #region Credit Cards
        [Route("paymentmethods/creditcards/edit/{type:alpha}")]
        public ActionResult ManageCreditCard(CreditCardType type)
        {
            var model = ExigoDAL.GetCustomerPaymentMethods(Identity.Customer.CustomerID)
                .Where(c => c is CreditCard && ((CreditCard)c).Type == type)
                .FirstOrDefault();

            // Clear out the card number
            ((CreditCard)model).CardNumber = "";

            return View("ManageCreditCard", model);
        }

        [Route("paymentmethods/creditcards/new")]
        public ActionResult AddCreditCard()
        {
            var model = new CreditCard();
            model.Type = CreditCardType.New;
            model.BillingAddress = new Address()
            {
                Country = CurrentMarket.MainCountry
            };

            return View("ManageCreditCard", model);
        }

        [Route("deletecreditcard")]
        public ActionResult DeleteCreditCard(CreditCardType type)
        {
            ExigoDAL.DeleteCustomerCreditCard(Identity.Customer.CustomerID, type);

            return RedirectToAction("PaymentMethodList");
        }

        [Route("savecreditcard")]
        [HttpPost]
        public ActionResult SaveCreditCard(CreditCard card)
        {
            card = ExigoDAL.SetCustomerCreditCard(Identity.Customer.CustomerID, card);

            return RedirectToAction("PaymentMethodList");
        }
        #endregion

        #region Bank Accounts
        [Route("paymentmethods/bankaccounts/edit/{type:alpha}")]
        public ActionResult ManageBankAccount(ExigoService.BankAccountType type)
        {
            var model = ExigoDAL.GetCustomerPaymentMethods(Identity.Customer.CustomerID)
                .Where(c => c is BankAccount && ((BankAccount)c).Type == type)
                .FirstOrDefault();


            // Clear out the account number
            ((BankAccount)model).AccountNumber = "";


            return View("ManageBankAccount", model);
        }

        [Route("paymentmethods/bankaccounts/new")]
        public ActionResult AddBankAccount()
        {
            var model = new BankAccount();
            model.Type = ExigoService.BankAccountType.New;
            model.BillingAddress = new Address()
            {
                Country = Identity.Customer.Market.MainCountry
            };

            return View("ManageBankAccount", model);
        }

        [Route("deletebankaccount")]
        public ActionResult DeleteBankAccount(ExigoService.BankAccountType type)
        {
            ExigoDAL.DeleteCustomerBankAccount(Identity.Customer.CustomerID, type);

            return RedirectToAction("PaymentMethodList");
        }

        [Route("savebankaccount")]
        [HttpPost]
        public ActionResult SaveBankAccount(BankAccount account)
        {
            account = ExigoDAL.SetCustomerBankAccount(Identity.Customer.CustomerID, account);

            return RedirectToAction("PaymentMethodList");
        }
        #endregion

        #endregion
        
        #region Signing in
        [AllowAnonymous]
        [Route("login")]
        public ActionResult Login()
        {
            var model = new LoginViewModel();

            return View(model);
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public JsonNetResult Login(LoginViewModel model)
        {
            var service = new IdentityService();
            var response = service.SignIn(model.LoginName, model.LoginPassword);

            return new JsonNetResult(response);
        }

        [Route("silentlogin")]
        [AllowAnonymous]
        public ActionResult SilentLogin(string token)
        {
            var service = new IdentityService();
            var response = service.SignIn(token);

            if (response.Status)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        #endregion

        #region Signing Out
        [Route("logout")]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        #endregion

        #region Models and Enums

        public class SearchResult
        {
            public int CustomerID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName
            {
                get { return this.FirstName + " " + this.LastName; }
            }
            public string AvatarURL { get; set; }
            public string WebAlias { get; set; }
            public string ReplicatedSiteUrl
            {
                get
                {
                    if (string.IsNullOrEmpty(this.WebAlias)) return "";
                    else return GlobalSettings.ReplicatedSites.GetFormattedUrl(WebAlias);
                }
            }

            public string MainState { get; set; }
            public string MainCity { get; set; }
            public string MainCountry { get; set; }
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
                var url = Url.Action("ResetPassword", "Account", new { token = token }, HttpContext.Request.Url.Scheme);
                var emailBody = Resources.Common.ForgotPasswordEmailBody.FormatWith(url);

                try
                {
                    //Send Email with Reset instructions
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
            catch (Exception e)
            {
                Log.Error(e, "Invalid Password Reset Token Used: {Message}", e.Message);
                return RedirectToAction("Login", "Authentication", new { error = "invalidtoken" });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            try
            {
                ExigoDAL.WebService().UpdateCustomer(new UpdateCustomerRequest()
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