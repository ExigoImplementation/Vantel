using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ExigoDevTools
{
    public class ExigoDevTools
    {
        public static MvcHtmlString InstantiateDevService() {
            return new MvcHtmlString(System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/Services/ExigoDevTools/DevToolView.html")));
        }
    }
}