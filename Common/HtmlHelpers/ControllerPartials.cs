using System.IO;
using System.Web.Mvc;

namespace Common.HtmlHelpers
{
    public static class ControllerPartials
    {
        public static MvcHtmlString ControllerPartial(this HtmlHelper html, string folder, string controller = "", object model = null)
        {
            // Determine which controller's navigation to get
            if (controller.IsNullOrEmpty())
            {
                controller = html.ViewContext.RouteData.Values["controller"].ToString();
            }

            // Check to see if the view exists
            var viewName = "~/Views/Shared/{0}/{1}.cshtml".FormatWith(folder, controller);
            ViewEngineResult result = ViewEngines.Engines.FindView(html.ViewContext.Controller.ControllerContext, viewName, null);

            if (result.View == null)
            {
                return new MvcHtmlString("Controller partial '{0}' not found".FormatWith(viewName));
            }
            else
            {
                html.ViewContext.Controller.ViewData.Model = model;


                using (var writer = new StringWriter())
                {
                    ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(html.ViewContext.Controller.ControllerContext, viewName);
                    ViewContext viewContext = new ViewContext(html.ViewContext.Controller.ControllerContext, viewResult.View, html.ViewContext.Controller.ViewData, html.ViewContext.Controller.TempData, writer);
                    viewResult.View.Render(viewContext, writer);

                    return new MvcHtmlString(writer.GetStringBuilder().ToString());
                }
            }
        }
    }
}