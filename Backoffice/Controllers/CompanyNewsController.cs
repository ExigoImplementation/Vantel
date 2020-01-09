using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using ExigoService;
using Backoffice.ViewModels;
using System.Threading.Tasks;

namespace Backoffice.Controllers
{
    public class CompanyNewsController : Controller
    {
        #region Company News
        [Route("~/news")]
        public ActionResult CompanyNewsList()
        {
            var model = new CompanyNewsViewModel();

            //Get all the departments that will be used for news
            var newsDepartments = GlobalSettings.Backoffices.CompanyNews.Departments;

            if (newsDepartments.Length > 0) // If there are departments one at a time grab all news from each department and pull them in.
            {
                model.CompanyNewsItems = ExigoDAL.GetCompanyNewsSQL(new GetCompanyNewsRequest() { NewsDepartments = newsDepartments });
            }
            else // none are configured grab them all by leaving the department blank
            {
                model.CompanyNewsItems = ExigoDAL.GetCompanyNewsSQL(new GetCompanyNewsRequest());
            }
            
            return View(model);
        }

        [Route("~/news/{newsid:int}")]
        public ActionResult CompanyNewsDetail(int newsid)
        {
            var model = new CompanyNewsViewModel();
            List<CompanyNewsItem> newsList = ExigoDAL.GetCompanyNewsSQL(new ExigoService.GetCompanyNewsRequest()
            {
                NewsDepartments = GlobalSettings.Backoffices.CompanyNews.Departments
            });

            model.CompanyNewsItems = newsList.Where(c => c.NewsID == newsid).ToList();

            return View(model);
        }
        #endregion
    }
}