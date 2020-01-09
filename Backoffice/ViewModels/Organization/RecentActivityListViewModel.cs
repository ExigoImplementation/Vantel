using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class RecentActivityListViewModel
    {
        public RecentActivityListViewModel()
        {
            this.RecentActivities = new List<CustomerWallItem>();
        }

        public List<CustomerWallItem> RecentActivities { get; set; }
    }
}