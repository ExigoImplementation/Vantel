using System;
using System.Collections.Generic;

namespace Backoffice.Models.SiteMap
{
    public interface INestableSiteMapNode<T>
    {
        IEnumerable<T> Children { get; set; }
    }
}
