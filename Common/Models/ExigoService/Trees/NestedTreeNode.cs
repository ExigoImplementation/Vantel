using System.Collections.Generic;
using System.Linq;

namespace ExigoService
{
    public class NestedTreeNode : TreeNode, INestedTreeNode<NestedTreeNode>
    {
        public NestedTreeNode()
        {
            Children = new List<NestedTreeNode>();
        }

        new public bool HasChildren
        {
            get { return this.Children.Count() > 0; }
        }
        new public int ChildNodeCount
        {
            get { return this.Children.Count(); }
            set { }
        }

        public List<NestedTreeNode> Children { get; set; }
    }
}