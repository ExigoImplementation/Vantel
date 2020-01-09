using System;
using System.Collections.Generic;
using System.Linq;

namespace Backoffice.Models.SiteMap
{
    public class NavigationSiteMap
    {
        /// <summary>
        /// The items in the site map.
        /// </summary>
        public List<ISiteMapNode> Items { get; set; }

        /// <summary>
        /// Recursively gets the first node matching the provided ID.
        /// </summary>
        /// <param name="id">The unique ID of the site map node.</param>
        /// <returns>The first matching site map node.</returns>
        public ISiteMapNode GetNode(string id)
        {
            return FirstOrDefaultNode(Items, id);
        }

        /// <summary>
        /// Determines if any node matching the provided ID is visible.
        /// </summary>
        /// <param name="id">The unique ID of the site map node.</param>
        /// <returns>Whether the node is invisible.</returns>
        public bool IsVisible(string id)
        {
            var node = GetNode(id);
            return node != null && node.IsVisible();
        }


        private ISiteMapNode FirstOrDefaultNode(IEnumerable<ISiteMapNode> source, string id)
        {
            var result = source.FirstOrDefault(c => c.ID == id);
            if (result != null) return result;

            foreach (var sourceItem in source.Where(c => c is INestableSiteMapNode<ISiteMapNode>))
            {
                var typedSourceItem = sourceItem as INestableSiteMapNode<ISiteMapNode>;
                if (typedSourceItem != null)
                {
                    result = FirstOrDefaultNode(typedSourceItem.Children, id);
                    if (result != null) break;
                }
            }

            return result;
        }
    }
}
