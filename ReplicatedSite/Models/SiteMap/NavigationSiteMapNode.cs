using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ReplicatedSite.Models.SiteMap
{
    /// <summary>
    /// A node in a site map.
    /// </summary>
    public class NavigationSiteMapNode : ISiteMapNode, INestableSiteMapNode<ISiteMapNode>
    {
        protected string DisabledUrl = "javascript:;";
        protected string DefaultTarget = "_self";


        public NavigationSiteMapNode(IEnumerable<ISiteMapNode> children = null)
        {
            Children  = new List<NavigationSiteMapNode>();
            Target = DefaultTarget;

            IsVisible = () => true;
            IsEnabled = () => true;
            IsActive = () => HttpContext.Current.Request.Url.AbsolutePath.Equals(Url);

            if (children != null) Children = children;
        }
        public NavigationSiteMapNode(string id, IEnumerable<ISiteMapNode> children = null)
            : this(children)
        {
            ID = id;
        }
        public NavigationSiteMapNode(string id, string label, IEnumerable<ISiteMapNode> children = null)
            : this(id, children)
        {
            Label = label;
        }

        /// <summary>
        /// The unique identifier for this node.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The CSS class used to identity which icon should be used with this node.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Determines if this node has an icon defined. 
        /// </summary>
        public bool HasIcon {
            get { return !string.IsNullOrEmpty(Icon); }
        }

        /// <summary>
        /// The displayable text of the node.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The HTML target for the node. Defaults to "_self".
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// The CSS class(es) that define this node's visibility across multiple devices. Examples: visible-xs, hidden-lg, etc.
        /// </summary>
        public string DeviceVisibilityCssClass { get; set; }

        /// <summary>
        /// The MVC action this node points to.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// The MVC controller this node points to.
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// (optional The MVC route parameters this node uses when linking to it's controller and action.
        /// </summary>
        public object RouteParameters { get; set; }

        /// <summary>
        /// The URL used for this node. This will be either the explicitly-provided URL or the provided MVC route.
        /// If this node is disabled, the DisabledUrl will be returned instead. 
        /// The DisabledUrl default is 'javascript:;'.
        /// </summary>
        public string Url
        {
            get
            {
                var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);

                if (!IsEnabled()) return DisabledUrl;

                if (!string.IsNullOrEmpty(_url))
                {
                    return _url;
                }

                if (!string.IsNullOrEmpty(Action) && !string.IsNullOrEmpty(Controller))
                {
                    return urlHelper.Action(Action, Controller, RouteParameters);
                }

                return DisabledUrl;
            }
            set { _url = value; }
        }
        private string _url;

        /// <summary>
        /// Determines if this node should be visible.
        /// </summary>
        public Func<bool> IsVisible { get; set; }

        /// <summary>
        /// Determines if this node should link to it's intended destination.
        /// </summary>
        public Func<bool> IsEnabled { get; set; }

        /// <summary>
        /// Determines if this node is considered active within the context of the request. If the current URL matches this nodes URL, it is considered active.
        /// </summary>
        public Func<bool> IsActive { get; set; }

        /// <summary>
        /// Any child nodes nested within this node.
        /// </summary>
        public IEnumerable<ISiteMapNode> Children { get; set; }
    }
}
