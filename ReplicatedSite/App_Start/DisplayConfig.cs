using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;

namespace ReplicatedSite
{
    public class DisplayConfig
    {
        public static void RegisterDisplayModes(IList<IDisplayMode> displayModes)
        {
            // The default mode
            var defaultMode = displayModes.Where(c => c.DisplayModeId == "").FirstOrDefault();

            // The mobile view
            var mobileMode = displayModes.Where(c => c.DisplayModeId == "Mobile").FirstOrDefault();

            displayModes.Clear();
            displayModes.Add(mobileMode);
            displayModes.Add(defaultMode);
        }
    }
}