using System.Web.Mvc;

namespace ReplicatedSite.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Index()
        {
            return UnexpectedError();
        }

        public ActionResult UnexpectedError()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult WebAliasRequired()
        {
            return View();
        }
        public ActionResult InvalidWebAlias()
        {
            return View();
        }
    }
}
