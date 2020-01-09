using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Common.HtmlHelpers
{
    public static class EditorsHtmlHelpers
    {
        public static RouteValueDictionary GetEditorHtmlAttributes(this HtmlHelper html, object defaults = null)
        {
            var viewData = html.ViewData;
            var results = new RouteValueDictionary();


            // Determine where all of our values are coming from
            var valueCollections = new List<RouteValueDictionary>();
            if (defaults != null) valueCollections.Add(new RouteValueDictionary(defaults));
            if (viewData["htmlAttributes"] != null) valueCollections.Add(new RouteValueDictionary(viewData["htmlAttributes"]));


            // Add some global attributes, if applicable
            var globalValues = new RouteValueDictionary();
            //globalValues.Add("placeholder", html.ViewContext.ViewData.ModelMetadata.DisplayName);
            if (html.ViewContext.ViewData.ModelMetadata.IsRequired)
            {
                globalValues.Add("required", "true");
                globalValues.Add("aria-required", "true");
            }
            if (globalValues.Count > 0) valueCollections.Add(globalValues);


            // Merge the collections together
            if (valueCollections.Count > 0)
            {
                foreach (var valueCollection in valueCollections)
                {
                    foreach (var item in valueCollection)
                    {
                        var key = item.Key.ToLower().Replace("_", "-");

                        if (results.ContainsKey(key))
                        {
                            results[key] = string.Format("{0} {1}", results[key], item.Value);
                        }
                        else
                        {
                            results.Add(key, item.Value);
                        }
                    }
                }
            }

            return results;
        }


        public static IDisposable BeginHtmlFieldPrefixScope(this HtmlHelper html, string htmlFieldPrefix)
        {
            return new HtmlFieldPrefixScope(html.ViewData.TemplateInfo, htmlFieldPrefix);
        }
        private class HtmlFieldPrefixScope : IDisposable
        {
            private readonly TemplateInfo templateInfo;
            private readonly string previousHtmlFieldPrefix;

            public HtmlFieldPrefixScope(TemplateInfo templateInfo, string htmlFieldPrefix)
            {
                this.templateInfo = templateInfo;

                previousHtmlFieldPrefix = templateInfo.HtmlFieldPrefix;
                templateInfo.HtmlFieldPrefix = htmlFieldPrefix;
            }

            public void Dispose()
            {
                templateInfo.HtmlFieldPrefix = previousHtmlFieldPrefix;
            }
        }
    }
}