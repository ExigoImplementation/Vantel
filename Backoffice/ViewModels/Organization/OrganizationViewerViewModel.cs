using ExigoService;
using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class OrganizationViewerViewModel
    {
        public IEnumerable<TreeNode> TreeNodes { get; set; }
        public TreeNode BottomLeftTreeNode { get; set; }

        public IEnumerable<Customer> WaitingRoomCustomers { get; set; }

        public bool IsUsingWaitingRoom { get; set; }
    }
}