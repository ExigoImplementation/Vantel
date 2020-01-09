using ExigoService;
using System;
using System.Collections.Generic;

namespace Backoffice.Models
{
    public class UnilevelNestedTreeNode : TreeNode, INestedTreeNode<UnilevelNestedTreeNode>
    {
        public UnilevelNestedTreeNode()
        {
            Customer    = new Customer();
            CurrentRank = new Rank();
        
            Children = new List<UnilevelNestedTreeNode>();
        }

        public List<UnilevelNestedTreeNode> Children { get; set; }

        public Customer Customer { get; set; }
        public Rank CurrentRank { get; set; }
        public bool IsPersonallyEnrolled { get; set; }
        public bool HasAutoOrder { get; set; }
        public bool Loaded { get; set; }

        // Unique Identity.Current.CustomerID encrypted ID for Profile popup action and tree viewer links
        public string ProfileID { get; set; }

    }

}