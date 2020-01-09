using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Web.Http;

namespace Backoffice
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{action}/{id}",
                defaults: new { controller = "WebApi", id = RouteParameter.Optional }
            );

            // Default all API calls to return JSON
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
            
            // Format all JSON to be camel-cased
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }

    /// <summary>
    /// Resolves property names returned in JSON responses to be formatted in lowercase.
    /// </summary>
    internal class LowerCasePropertyNamesContractResolver : DefaultContractResolver
    {
        public LowerCasePropertyNamesContractResolver() : base()
        {
        }
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLowerInvariant();
        }      
    }
}
