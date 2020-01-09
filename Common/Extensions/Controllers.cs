using System.IO;
using System.Web.Mvc;

public static class ControllerExtensions
{
    /// <summary>
    /// Method Used to Render a Partial View with Model intact as a string for use in a JSON request
    /// </summary>
    /// <param name="viewName">Path of view that you are attempting to call.</param>
    /// <param name="model">The model data that the view should expect to receive.</param>
    /// <returns>Html string of partial view.</returns>
    public static string RenderPartialViewToString(this Controller controller, string viewName, object model)
    {
        if (string.IsNullOrEmpty(viewName))
            viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

        controller.ViewData.Model = model;


        using (var writer = new StringWriter())
        {
            ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
            ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, writer);
            viewResult.View.Render(viewContext, writer);

            return writer.GetStringBuilder().ToString();
        }
    }

    public static string RenderPartialViewToString(this Controller controller, string viewName, object model, ViewDataDictionary viewData)
    {
        if (string.IsNullOrEmpty(viewName))
            viewName = controller.ControllerContext.RouteData.GetRequiredString("action");

        controller.ViewData.Model = model;
        foreach (var item in viewData)
        {
            controller.ViewData.Add(item);
        }

        using (var writer = new StringWriter())
        {
            ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
            ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, writer);
            viewResult.View.Render(viewContext, writer);

            return writer.GetStringBuilder().ToString();
        }
    }

    /// <summary>
    /// Determines if the provided view exists for the this controller
    /// </summary>
    /// <param name="view">The name of the view</param>
    /// <returns></returns>
    public static bool ViewExists(this Controller controller, string view)
    {
        ViewEngineResult result = ViewEngines.Engines.FindView(controller.ControllerContext, view, null);
        return (result.View != null);
    }
}