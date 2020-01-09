using System;

namespace Backoffice.Models.SiteMap
{
    public class DividerNode : ISiteMapNode
    {
        public DividerNode()
        {
            ID = Guid.NewGuid().ToString();

            IsVisible = () => true;
        }

        public string ID { get; set; }
        public string Label { get; set; }

        public Func<bool> IsVisible { get; set; }
    }
}
