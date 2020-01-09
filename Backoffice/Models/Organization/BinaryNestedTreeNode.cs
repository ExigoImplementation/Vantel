using ExigoService;
using System.Collections.Generic;

namespace Backoffice.Models
{
    public class BinaryNestedTreeNode : TreeNode, INestedTreeNode<BinaryNestedTreeNode>
    {
        public BinaryNestedTreeNode()
        {
            this.Customer    = new Customer();
            this.CurrentRank = new Rank();

            this.Children = new List<BinaryNestedTreeNode>();
        }

        public List<BinaryNestedTreeNode> Children { get; set; }

        public Customer Customer { get; set; }
        public Rank CurrentRank { get; set; }
        public bool IsPersonallyEnrolled { get; set; }
        public bool HasAutoOrder { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
    }
}