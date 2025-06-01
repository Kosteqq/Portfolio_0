
namespace ProjectPortfolio.Paths
{
    internal class PathfinderNode
    {
        public readonly GridNode GridNode;
        public readonly PathfinderNode[] Neighbours;
        
        public PathfinderNodeKey Key;
        public float Rhs = float.MaxValue;
        public float G = float.MaxValue;

        public bool IsRhsEqualG => Rhs.IsApprox(G);

        public PathfinderNode(GridNode p_node)
        {
            GridNode = p_node;
            Neighbours = new PathfinderNode[p_node.Neighbours.Count];
        }
    }
}