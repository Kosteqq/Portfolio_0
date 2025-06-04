using System.Collections.Generic;
using System.Linq;
using ProjectPortfolio.Global;

namespace ProjectPortfolio.Paths
{
    internal class PathfinderQueue
    {
        private class NodeSortComparer : IComparer<PathfinderNode>
        {
            public int Compare(PathfinderNode p_first, PathfinderNode p_second)
            {
                if (p_first!.Key < p_second!.Key)
                    return -1;

                if (p_first.Key > p_second.Key)
                    return 1;

                return 0;
            }
        }
            
        private readonly List<PathfinderNode> _sortedNode = new();
            
        public PathfinderNode First => _sortedNode.OrderBy(node => node, new NodeSortComparer()).First();
        public bool IsEmpty => _sortedNode.IsEmpty();

        public void Enqueue(PathfinderNode p_node, PathfinderNodeKey p_key)
        {
            p_node.Key = p_key;
            _sortedNode.Add(p_node);
        }

        public PathfinderNode Dequeue()
        {
            PathfinderNode first = First;
            _sortedNode.Remove(first);
            return first;
        }

        public void Clear()
        {
            _sortedNode.Clear();
        }

        public bool Contains(PathfinderNode p_node)
        {
            return _sortedNode.Contains(p_node);
        }

        public void Remove(PathfinderNode p_node)
        {
            _sortedNode.Remove(p_node);
        }

        public void Update(PathfinderNode p_node, PathfinderNodeKey p_newKey)
        {
            if (_sortedNode.Remove(p_node))
            {
                p_node.Key = p_newKey;
                _sortedNode.Add(p_node);
            }
        }
    }
}