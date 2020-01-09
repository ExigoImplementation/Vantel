using System;

namespace Backoffice.Models.SiteMap
{
    public class HeadingNode : ISiteMapNode
    {
        public HeadingNode(string label)
        {
            ID    = Guid.NewGuid().ToString();
            Label = label;

            IsVisible = () => true;
        }

        public string ID { get; set; }
        public string Label { get; set; }

        public Func<bool> IsVisible { get; set; }
    }
}
