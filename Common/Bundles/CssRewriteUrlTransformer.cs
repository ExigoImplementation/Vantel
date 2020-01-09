using System.Web;
using System.Web.Optimization;


namespace Common.Bundles
{
    public class CssRewriteUrlTransformer : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
        }
    }
}
