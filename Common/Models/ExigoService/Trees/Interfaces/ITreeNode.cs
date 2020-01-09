using System;

namespace ExigoService
{
    public interface ITreeNode
    {
        Guid NodeID { get; }
        int CustomerID { get; set; }

        Guid ParentNodeID { get; set; }
        int ParentCustomerID { get; set; }

        int Level { get; set; }
        int PlacementID { get; set; }
        int IndentedSort { get; set; }
        int ChildNodeCount { get; set; }

        bool IsNullPosition { get; set; }
        bool IsOpenPosition { get; set; }
    }
}