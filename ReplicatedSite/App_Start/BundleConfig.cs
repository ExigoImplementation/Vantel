using Common.Bundles;
using System.Web.Optimization;

namespace ReplicatedSite
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Enable bundling optimizations, even when the site is in debug mode or local.
            BundleTable.EnableOptimizations = true;


            // Bundle the Handlebars plugins
            var handlebarsScripts = new ScriptBundle("~/bundles/scripts/handlebars");
            handlebarsScripts.Include(
                "~/Content/scripts/vendor/handlebars.js",
                "~/Content/scripts/vendor/handlebars.extended.js");
            handlebarsScripts.Orderer = new NonOrderingBundleOrderer();

            bundles.Add(handlebarsScripts);



            // Bundle the vendor's styles
            var vendorStyles = new StyleBundle("~/bundles/styles/vendor");   
            bundles.Add(vendorStyles);

        }
    }
}