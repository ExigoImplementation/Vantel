using System.Collections.Generic;

namespace ExigoService
{
    public interface INestedTreeNode<T> : ITreeNode
    {
        List<T> Children { get; set; }
    }
}