using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class DashboardViewModel
    {
        public List<Order> RecentOrders { get; set; }
        public VolumeCollection Volumes { get; set; }
        public List<RealTimeCommission> CurrentCommissions { get; set; }
        public List<Customer> NewestDistributors { get; set; }
        public List<CustomerWallItem> RecentActivities { get; set; }
        public List<CompanyNewsItem> CompanyNews { get; set; }
    }
}