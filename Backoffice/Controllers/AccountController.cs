using Backoffice.Models.CommissionPayout;
using Backoffice.Services;
using Backoffice.ViewModels;
using Common;
using Common.Api.ExigoWebService;
using Common.Filters;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Backoffice.Controllers
{
    [RoutePrefix("account")]
    [Route("{action=index}")]
    public class AccountController : BaseController
    {
        #region Overview
        [Route("settings")]
        public ActionResult Index()
        {
            var model = new AccountOverviewViewModel();

            var customer = Customers.GetCustomer(Identity.Current.CustomerID);
            var website = ExigoDAL.GetCustomerSiteRealTime(Identity.Current.CustomerID);
            var socialNetworksResponse = ExigoDAL.GetCustomerSocialNetwork(Identity.Current.CustomerID);
            //var socialNetworksResponse = Exigo.WebService().GetCustomerSocialNetworks(new GetCustomerSocialNetworksRequest()
            //{
            //    CustomerID = Identity.Current.CustomerID
            //});

            foreach (var network in socialNetworksResponse)
            {
                switch (network.SocialNetworkID)
                {
                    case (int)SocialNetworks.Facebook: model.Facebook = network.Url; break;
                    case (int)SocialNetworks.Twitter: model.Twitter = network.Url; break;
                    case (int)SocialNetworks.YouTube: model.YouTube = network.Url; break;
                    case (int)SocialNetworks.Blog: model.Blog = network.Url; break;
                    case (int)SocialNetworks.GooglePlus: model.GooglePlus = network.Url; break;
                    case (int)SocialNetworks.LinkedIn: model.LinkedIn = network.Url; break;
                    case (int)SocialNetworks.MySpace: model.MySpace = network.Url; break;
                    case (int)SocialNetworks.Pinterest: model.Pinterest = network.Url; break;
                    case (int)SocialNetworks.Instagram: model.Instagram = network.Url; break;
                }
            }

            model.Customer = customer;

            model.WebAlias = website.WebAlias;

            model.CustomerSite.FirstName = website.FirstName;
            model.CustomerSite.LastName = website.LastName;
            model.CustomerSite.Email = website.Email;
            model.CustomerSite.PrimaryPhone = website.PrimaryPhone;
            model.CustomerSite.SecondaryPhone = website.SecondaryPhone;
            model.CustomerSite.Fax = website.Fax;
            model.CustomerSite.Notes1 = website.Notes1;
            model.CustomerSite.Notes2 = website.Notes2;
            model.CustomerSite.Notes3 = website.Notes3;
            model.CustomerSite.Notes4 = website.Notes4;
            model.CustomerSite.Address.Address1 = website.Address.Address1;
            model.CustomerSite.Address.Address2 = website.Address.Address2;
            model.CustomerSite.Address.Country = website.Address.Country;
            model.CustomerSite.Address.City = website.Address.City;
            model.CustomerSite.Address.State = website.Address.State;
            model.CustomerSite.Address.Zip = website.Address.Zip;

            return View(model);
        }

        // Account settings
        [HttpParamAction]
        public JsonNetResult UpdateEmailAddress(Customer customer)
        {
            ExigoDAL.UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = Identity.Current.CustomerID,
                Email = customer.Email
            });

            ExigoDAL.SendEmailVerification(Identity.Current.CustomerID, customer.Email);

            var html = string.Format("{0}", customer.Email);

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateEmailAddress",
                html = html
            });
        }

        [HttpParamAction]
        public JsonNetResult UpdateWebAlias(string webalias)
        {
            ExigoDAL.WebService().SetCustomerSite(new SetCustomerSiteRequest
            {
                CustomerID = Identity.Current.CustomerID,
                WebAlias = webalias
            });

            var html = string.Format("<a href='" + GlobalSettings.Company.BaseReplicatedUrl + "/" + webalias + "'>" + GlobalSettings.Company.BaseReplicatedUrl + "/<strong>{0}</strong></a>", webalias);

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateWebAlias",
                html = html
            });
        }
        public JsonResult IsValidWebAlias(string webalias)
        {
            var isValid = ExigoDAL.IsWebAliasAvailable(Identity.Current.CustomerID, webalias);

            if (isValid) return Json(true, JsonRequestBehavior.AllowGet);
            else return Json(string.Format(Resources.Common.PasswordNotAvailable, webalias), JsonRequestBehavior.AllowGet);
        }
        
        [HttpParamAction]
        public JsonNetResult UpdateLoginName(Customer customer)
        {
            ExigoDAL.UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = Identity.Current.CustomerID,
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
            var isValid = ExigoDAL.IsLoginNameAvailable(loginname, Identity.Current.CustomerID);

            if (isValid) return Json(true, JsonRequestBehavior.AllowGet);
            else return Json(string.Format(Resources.Common.LoginNameNotAvailable, loginname), JsonRequestBehavior.AllowGet);
        }

        [HttpParamAction]
        public JsonNetResult UpdatePassword(string password)
        {
            ExigoDAL.UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID    = Identity.Current.CustomerID,
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

            ExigoDAL.SetCustomerPreferredLanguage(Identity.Current.CustomerID, language.LanguageID);

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
                CustomerID = Identity.Current.CustomerID,
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

        [HttpParamAction]
        public JsonNetResult UpdateMobilePhone(Customer customer)
        {
            ExigoDAL.UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = Identity.Current.CustomerID,
                MobilePhone = customer.MobilePhone
            });

            var html = string.Format("{1}: <strong>{0}</strong>", customer.MobilePhone, Resources.Common.SendTextsTo);

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateMobilePhone",
                html = html
            });
        }

        [HttpParamAction]
        public JsonNetResult UpdateFaxNumber(Customer customer)
        {
            ExigoDAL.UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = Identity.Current.CustomerID,
                Fax = customer.Fax
            });

            var html = string.Format("{1}: <strong>{0}</strong>", customer.Fax, Resources.Common.SendFaxesTo);

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateFaxNumber",
                html = html
            });
        }

        // Website settings
        [HttpParamAction]
        public JsonNetResult UpdateWebsiteName(CustomerSite customersite)
        {
            ExigoDAL.UpdateCustomerSite(new CustomerSite
            {
                CustomerID = Identity.Current.CustomerID,
                FirstName = customersite.FirstName,
                LastName = customersite.LastName
            });

            var html = string.Format("{0} {1}", customersite.FirstName, customersite.LastName);

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateWebsiteName",
                html = html
            });
        }

        [HttpParamAction]
        public JsonNetResult UpdateWebsiteEmail(CustomerSite customersite)
        {
            ExigoDAL.UpdateCustomerSite(new CustomerSite
            {
                CustomerID = Identity.Current.CustomerID,
                Email = customersite.Email
            });

            var html = string.Format("{0}", customersite.Email);

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateWebsiteEmail",
                html = html
            });
        }

        [HttpParamAction]
        public JsonNetResult UpdateWebsitePhoneNumbers(CustomerSite customersite)
        {

            var testa = ExigoDAL.GetCustomerSite(Identity.Current.CustomerID);

            ExigoDAL.UpdateCustomerSite(new CustomerSite
            {
                CustomerID = Identity.Current.CustomerID,
                PrimaryPhone = customersite.PrimaryPhone,
                SecondaryPhone = customersite.SecondaryPhone
            });

            var testb = ExigoDAL.GetCustomerSite(Identity.Current.CustomerID);

            var html = string.Format(@"
                " + Resources.Common.Primary + @": <strong>{0}</strong><br />
                " + Resources.Common.Secondary + @": <strong>{1}</strong>
                ", customersite.PrimaryPhone, customersite.SecondaryPhone);

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateWebsitePhoneNumbers",
                html = html
            });
        }

        [HttpParamAction]
        public JsonNetResult UpdateWebsiteFax(CustomerSite customersite)
        {
            ExigoDAL.UpdateCustomerSite(new CustomerSite
            {
                CustomerID = Identity.Current.CustomerID,
                Fax = customersite.Fax
            });

            var html = string.Format("{0}", customersite.Fax);

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateWebsiteFax",
                html = html
            });
        }

        [HttpParamAction]
        public JsonNetResult UpdateWebsiteAddress(CustomerSite customersite)
        {
            // Attempt to validate the user's entered address if US address
            customersite.Address = GlobalUtilities.ValidateAddress(customersite.Address) as Address;

            ExigoDAL.UpdateCustomerSite(new CustomerSite
            {
                CustomerID = Identity.Current.CustomerID,
                Address = customersite.Address
            });


            var html = customersite.Address.AddressDisplay + "<br />" + customersite.Address.City + ", " + customersite.Address.State + " " + customersite.Address.Zip + ", " + customersite.Address.Country;

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateWebsiteAddress",
                html = html
            });
        }

        [HttpParamAction]
        public JsonNetResult UpdateWebsiteMessage(CustomerSite customersite)
        {
            ExigoDAL.UpdateCustomerSite(new CustomerSite
            {
                CustomerID = Identity.Current.CustomerID,
                Notes1 = customersite.Notes1
            });

            var html = "<p>" + customersite.Notes1 + "</p>";

            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateWebsiteMessage",
                html = html
            });
        }

        [HttpParamAction]
        public JsonNetResult UpdateWebsiteSocialMediaLinks(string facebook, string twitter, string youtube, string blog, string googleplus, string linkedin, string myspace, string pinterest, string instagram)
        {
            var socialrequest = new SetCustomerSocialNetworksRequest();
            socialrequest.CustomerID = Identity.Current.CustomerID;
            // Prefix all social media urls with https if does not set
            facebook = facebook.ToHttpPrefixedURI();
            twitter = twitter.ToHttpPrefixedURI();
            youtube = youtube.ToHttpPrefixedURI();
            blog = blog.ToHttpPrefixedURI();
            googleplus = googleplus.ToHttpPrefixedURI();
            linkedin = linkedin.ToHttpPrefixedURI();
            myspace = myspace.ToHttpPrefixedURI();
            pinterest = pinterest.ToHttpPrefixedURI();
            instagram = instagram.ToHttpPrefixedURI();

            var urls = new List<CustomerSocialNetworkRequest>();
            if (!string.IsNullOrEmpty(facebook)) urls.Add(new CustomerSocialNetworkRequest() { SocialNetworkID = (int)SocialNetworks.Facebook, Url = facebook });
            if (!string.IsNullOrEmpty(twitter)) urls.Add(new CustomerSocialNetworkRequest() { SocialNetworkID = (int)SocialNetworks.Twitter, Url = twitter });
            if (!string.IsNullOrEmpty(youtube)) urls.Add(new CustomerSocialNetworkRequest() { SocialNetworkID = (int)SocialNetworks.YouTube, Url = youtube });
            if (!string.IsNullOrEmpty(blog)) urls.Add(new CustomerSocialNetworkRequest() { SocialNetworkID = (int)SocialNetworks.Blog, Url = blog });
            if (!string.IsNullOrEmpty(googleplus)) urls.Add(new CustomerSocialNetworkRequest() { SocialNetworkID = (int)SocialNetworks.GooglePlus, Url = googleplus });
            if (!string.IsNullOrEmpty(linkedin)) urls.Add(new CustomerSocialNetworkRequest() { SocialNetworkID = (int)SocialNetworks.LinkedIn, Url = linkedin });
            if (!string.IsNullOrEmpty(myspace)) urls.Add(new CustomerSocialNetworkRequest() { SocialNetworkID = (int)SocialNetworks.MySpace, Url = myspace });
            if (!string.IsNullOrEmpty(pinterest)) urls.Add(new CustomerSocialNetworkRequest() { SocialNetworkID = (int)SocialNetworks.Pinterest, Url = pinterest });
            if (!string.IsNullOrEmpty(instagram)) urls.Add(new CustomerSocialNetworkRequest() { SocialNetworkID = (int)SocialNetworks.Instagram, Url = instagram });
            socialrequest.CustomerSocialNetworks = urls.ToArray();

            ExigoDAL.WebService().SetCustomerSocialNetworks(socialrequest);

             facebook = (facebook.IsNotNullOrEmpty()) ? string.Format("<a href='{0}' target='_blank'><strong>{0}</strong></a>", facebook) : "&nbsp;"; 
             twitter = (twitter.IsNotNullOrEmpty()) ? string.Format("<a href='{0}' target='_blank'><strong>{0}</strong></a>", twitter) : "&nbsp;"; 
             youtube = (youtube.IsNotNullOrEmpty()) ? string.Format("<a href='{0}' target='_blank'><strong>{0}</strong></a>", youtube) : "&nbsp;"; 
             blog = (blog.IsNotNullOrEmpty()) ? string.Format("<a href='{0}' target='_blank'><strong>{0}</strong></a>", blog) : "&nbsp;";
             googleplus = (googleplus.IsNotNullOrEmpty()) ? string.Format("<a href='{0}' target='_blank'><strong>{0}</strong></a>", googleplus) : "&nbsp;";
             linkedin = (linkedin.IsNotNullOrEmpty()) ? string.Format("<a href='{0}' target='_blank'><strong>{0}</strong></a>", linkedin) : "&nbsp;";
             myspace = (myspace.IsNotNullOrEmpty()) ? string.Format("<a href='{0}' target='_blank'><strong>{0}</strong></a>", myspace) : "&nbsp;";
             pinterest = (pinterest.IsNotNullOrEmpty()) ? string.Format("<a href='{0}' target='_blank'><strong>{0}</strong></a>", pinterest) : "&nbsp;";
             instagram = (instagram.IsNotNullOrEmpty()) ? string.Format("<a href='{0}' target='_blank'><strong>{0}</strong></a>", instagram) : "&nbsp;"; 

            var html = string.Format(@"
                <dl class='dl-metric'>
                    <dt>" + Resources.Common.Facebook + @":</dt>
                    <dd>" + facebook + @"</dd>
                    <dt>" + Resources.Common.Twitter + @":</dt>
                    <dd>" + twitter + @"</dd>
                    <dt>" + Resources.Common.YouTube + @":</dt>
                    <dd>" + youtube + @"</dd>
                    <dt>" + Resources.Common.Blog + @":</dt>
                    <dd>" + blog + @"</dd>
                    <dt>" + Resources.Common.GooglePlus + @":</dt>
                    <dd>" + googleplus + @"</dd>
                    <dt>" + Resources.Common.LinkedIn + @":</dt>
                    <dd>" + linkedin + @"</dd>
                    <dt>" + Resources.Common.MySpace + @":</dt>
                    <dd>" + myspace + @"</dd>
                    <dt>" + Resources.Common.Pinterest + @":</dt>
                    <dd>" + pinterest + @"</dd>
                    <dt>" + Resources.Common.Instagram + @":</dt>
                    <dd>" + instagram + @"</dd>
                </dl>
                ", facebook, twitter, youtube, blog, googleplus, linkedin, myspace, pinterest, instagram);
            return new JsonNetResult(new
            {
                success = true,
                action = "UpdateWebsiteSocialMediaLinks",
                html = html
            });
        }
        #endregion

        #region Addresses
        [Route("addresses")]
        public ActionResult AddressList()
        {
            var model = Customers.GetCustomerAddresses(Identity.Current.CustomerID).Where(c => c.IsComplete).ToList();

            return View(model);
        }

        [Route("addresses/edit/{type:alpha}")]
        public ActionResult ManageAddress(AddressType type)
        {
            var model = Customers.GetCustomerAddresses(Identity.Current.CustomerID).Where(c => c.AddressType == type).FirstOrDefault();

            return View("ManageAddress", model);
        }

        [Route("addresses/new")]
        public ActionResult AddAddress()
        {
            var model         = new Address();
            model.AddressType = AddressType.New;
            model.Country     = CurrentMarket.MainCountry;

            return View("ManageAddress", model);
        }

        public ActionResult DeleteAddress(AddressType type)
        {
            ExigoDAL.DeleteCustomerAddress(Identity.Current.CustomerID, type);

            return RedirectToAction("AddressList");
        }

        public ActionResult SetPrimaryAddress(AddressType type)
        {
            ExigoDAL.SetCustomerPrimaryAddress(Identity.Current.CustomerID, type);

            return RedirectToAction("AddressList");
        }

        [HttpPost]
        public ActionResult SaveAddress(Address address)
        {
            ExigoDAL.SetCustomerAddressOnFile(Identity.Current.CustomerID, address);

            return RedirectToAction("AddressList");
        }
        #endregion

        #region Payment Methods
        [Route("paymentmethods")]
        public ActionResult PaymentMethodList()
        {
            var model = ExigoDAL.GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
            {
                CustomerID               = Identity.Current.CustomerID,
                ExcludeIncompleteMethods = true
            });

            return View(model);
        }

        #region Credit Cards
        [Route("paymentmethods/creditcards/edit/{type:alpha}")]
        public ActionResult ManageCreditCard(CreditCardType type)
        {
            var model = ExigoDAL.GetCustomerPaymentMethods(Identity.Current.CustomerID)
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
                Country = CurrentMarket.MainCountry            };

            return View("ManageCreditCard", model);
        }

        public ActionResult DeleteCreditCard(CreditCardType type)
        {
            ExigoDAL.DeleteCustomerCreditCard(Identity.Current.CustomerID, type);

            return RedirectToAction("PaymentMethodList");
        }

        [HttpPost]
        public ActionResult SaveCreditCard(CreditCard card)
        {
            try
            {
                card = ExigoDAL.SetCustomerCreditCard(Identity.Current.CustomerID, card);

                return RedirectToAction("PaymentMethodList");
            }
            catch (Exception ex)
            {

                return RedirectToAction("PaymentMethodList", new { error = ex.Message.ToString() });
            }
        }
        #endregion

        #region Bank Accounts
        [Route("paymentmethods/bankaccounts/edit/{type:alpha}")]
        public ActionResult ManageBankAccount(ExigoService.BankAccountType type)
        {
            var model = ExigoDAL.GetCustomerPaymentMethods(Identity.Current.CustomerID)
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
                Country = CurrentMarket.MainCountry
            };

            return View("ManageBankAccount", model);
        }

        public ActionResult DeleteBankAccount(ExigoService.BankAccountType type)
        {
            ExigoDAL.DeleteCustomerBankAccount(Identity.Current.CustomerID, type);

            return RedirectToAction("PaymentMethodList");
        }

        [HttpPost]
        public ActionResult SaveBankAccount(BankAccount account)
        {
            account = ExigoDAL.SetCustomerBankAccount(Identity.Current.CustomerID, account);

            return RedirectToAction("PaymentMethodList");
        }
        #endregion

        #endregion

        #region Commission Payout
        public ActionResult CommissionPayout()
        {
            var bankaccount = Commissions.GetDirectDeposit();

            return View(bankaccount);
        }

        public ActionResult ManageCommissionPayout()
        {
            var bankaccount = Commissions.GetDirectDeposit();

            bankaccount.AccountNumber = "";

            return View(bankaccount);
        }

        [HttpPost]
        public ActionResult UpdateDirectDeposit(CommissionPayout account)
        {
            if (Commissions.SetDirectDeposit(account))
            {
                return RedirectToAction("commissionpayout", "account", new { success = true });
            }
            else
            {
                return RedirectToAction("managecommissionpayout", "account", new { success = false });
            }
        }
        #endregion
        
        #region Avatars
        public List<string> AllowableImageFormats = new List<string>()
        {
            "image/jpg",
            "image/jpeg",
            "image/pjpeg",
            "image/gif",
            "image/x-png",
            "image/png"
        };

        [Route("avatar")]
        public ActionResult ManageAvatar()
        {
            // Get the default avatars from the folder in this website
            try
            {
                var defaultAvatarPath = "~/Content/images/avatars";
                var baseUrl = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content(defaultAvatarPath));
                var defaultAvatarFiles = Directory.GetFiles(Server.MapPath(defaultAvatarPath));
                if (defaultAvatarFiles.Count() > 0)
                {
                    foreach (var url in defaultAvatarFiles)
                    {
                        var urlParts = url.Split('\\');
                        var filename = urlParts[urlParts.Length - 1];
                    }
                }
            }
            catch { }


            return View();
        }

        [HttpPost]
        public JsonNetResult SetAvatarFromFile(HttpPostedFileBase file = null)
        {
            if (file == null)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    message = "Please choose a File to upload"
                });
            }

            // Validate that the file is valid
            var isValidImage = (file != null && file.ContentLength > 0 && AllowableImageFormats.Contains(file.ContentType.ToLower()));
            if (isValidImage)
            {
                var maxWidth = 500;
                var maxHeight = 500;

                // Save the image
                var bytes = GlobalUtilities.GetBytesFromStream(file.InputStream);
                var resizedBytes = GlobalUtilities.ResizeImage(bytes, maxWidth, maxHeight);

                var html = this.RenderPartialViewToString("CropAvatar", resizedBytes);

                return new JsonNetResult(new
                {
                    success = isValidImage,
                    html
                });
            }

            return new JsonNetResult(new
            {
                success = isValidImage
            });
        }

        [HttpPost]
        [Route("avatar/crop")]
        public JsonNetResult CropAvatar(int width, int height, int x, int y, string rawBytes)
        {
            var bytes = Convert.FromBase64String(rawBytes);
            
            var croppedBytes = GlobalUtilities.Crop(bytes, width, height, x, y);

            ExigoDAL.Images().SetCustomerAvatar(Identity.Current.CustomerID, croppedBytes, true);


            return new JsonNetResult(new
            {
                success = true
            });
        }
        #endregion

        #region Email Notifications
        [Route("notifications")]
        public ActionResult Notifications()
        {
            var model = new AccountNotificationsViewModel();
            
            var customer    = Customers.GetCustomer(Identity.Current.CustomerID);
            model.Email     = customer.Email;
            model.IsOptedIn = customer.IsOptedIn;

            return View(model);
        }

        [Route("notifications/unsubscribe")]
        public ActionResult Unsubscribe()
        {
            ExigoDAL.OptOutCustomer(Identity.Current.CustomerID);

            return RedirectToAction("Notifications");
        }

        [HttpPost]
        public JsonNetResult SendEmailVerification(string email)
        {
            try
            {
                ExigoDAL.SendEmailVerification(Identity.Current.CustomerID, email);

                return new JsonNetResult(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                return new JsonNetResult(new
                {
                    success = false,
                    error = ex.Message
                });
            }            
        }

        [AllowAnonymous]
        [Route("~/verifyemail")]
        public ActionResult VerifyEmail(string token)
        {
            try
            {
                ExigoDAL.OptInCustomer(token);

                return View();
            }
            catch
            {
                throw new HttpException(404, "Invalid token");
            }
        }
        #endregion
    }
}