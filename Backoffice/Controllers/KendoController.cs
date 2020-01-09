using System;
using System.Web.Mvc;

namespace Backoffice.Controllers
{
    public class KendoController : Controller
    {
        [Route("~/kendo/grid/export/excel")]
        public FileResult ExportGridToExcel(string contentType, string base64, string fileName)
        {
            Response.Headers["Content-Disposition"] = "attachment; filename=" + fileName;
            return File(Convert.FromBase64String(base64), contentType, fileName);
        }

        [Route("~/kendo/grid/export/pdf")]
        public FileResult ExportGridToPDF(string contentType, string base64, string fileName)
        {
            Response.Headers["Content-Disposition"] = "attachment; filename=" + fileName;
            return File(Convert.FromBase64String(base64), contentType, fileName);
        }
    }
}
