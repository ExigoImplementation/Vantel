using ExigoService;
using System;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class ProfileViewModel
    {
        public Customer Customer { get; set; }
        public bool IsInEnrollerTree { get; set; }
        public IEnumerable<CustomerWallItem> RecentActivity { get; set; }
        public VolumeCollection Volumes { get; set; }
        public string Token { get; set; }
        public DateTime? NextOrderDate { get; set; }
    }
}