using Common;
using Common.Services;
using Dapper;
using ExigoService;
using ReplicatedSite.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace ReplicatedSite.Controllers
{
    public class HomeController : BaseController
    {
        
        public ActionResult Index()
        {
            var ids = GetDefaultHomePageProducts();
            var items = ExigoDAL.GetItems(new GetItemsRequest
            {
                Configuration = CurrentMarket.Configuration.Orders,
                LanguageID = CurrentLanguage.LanguageID,
                IgnoreCategoryRestrictions = true,
                ItemCodes = ids,
            }).ToList();

            return View(items);
        }

        [HttpGet]
        [Route("terms-and-conditions")]
        public ActionResult TermsAndConditions()
        {
            return View();
        }


        [HttpGet]
        [Route("privacy-policy")]
        public ActionResult PrivacyPolicy()
        {
            return View();
        }


        private List<string> GetDefaultHomePageProducts()
        {
            if (string.IsNullOrEmpty(GlobalSettings.ReplicatedSites.DefaultHomePageProducts))
            {
                using (var context = ExigoDAL.Sql())
                {
                    var sql = @"
                                SELECT top " + (GlobalSettings.ReplicatedSites.TopXHomePageProducts.IsNullOrEmpty() ? "5" : GlobalSettings.ReplicatedSites.TopXHomePageProducts) + @" 
                                    i.ItemCode
                                    FROM [dbo].[items] i 
	                                join [dbo].[WebCategoryItems] wci
	                                on i.ItemID = wci.ItemID " +
                                (GlobalSettings.ReplicatedSites.HomePageProductsWebCat.IsNullOrEmpty() ? "" : ("where wci.WebCategoryID = " + GlobalSettings.ReplicatedSites.HomePageProductsWebCat));

                    return context.Query<string>(sql).ToList();
                }
            }
            else return GlobalSettings.ReplicatedSites.DefaultHomePageProducts.Split(',').ToList();
        }

        public ActionResult About()
        {
            var model = new ContactViewModel
            {
                SiteOwnerNotes = Identity.Owner.Notes1.IsNotNullOrEmpty() ? Identity.Owner.Notes1 : Resources.Common.WellcomeToMySiteMessage
            };
            return View(model);
        }

        public ActionResult ContactUs()
        {
            var model = new ContactViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult ContactUs(ContactViewModel model)
        {
            try
            {
                //Identity.Owner.
                var requestUrl = Request.Url.AbsoluteUri;

                // Send the email
                ExigoDAL.SendEmail(new SendEmailRequest
                {
                    //Change to ContactUsEmail for production
                    To = new[] { GlobalSettings.Company.Email },
                    // Use the WebService Send Email call when no SMTP credentials are present
                    UseExigoApi = true,
                    From = model.Email,
                    Subject = "Contact Us Email",
                    Body = @"
                        <h3>Contact Us Email</h3>    
                         <ul>
                            <li>Name: {0}</li>
                            <li>Phone: {1}</li>
                            <li>Email: {2}</li>
                            <li>Notes: {3}</li>
                            <li>From Url: {4}</li>
                         </ul>                                   
                    ".FormatWith(model.Name, model.Phone, model.Email, model.Notes, requestUrl)
                });

                return new JsonNetResult(new
                {
                    success = true,
                    model
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
    }
}