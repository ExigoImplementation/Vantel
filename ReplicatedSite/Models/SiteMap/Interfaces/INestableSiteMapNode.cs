using System;
using System.Collections.Generic;

namespace ReplicatedSite.Models.SiteMap
{
    public interface INestableSiteMapNode<T>
    {
        IEnumerable<T> Children { get; set; }
    }
}
