using System.Collections.Generic;
using JetBrains.Annotations;
using ProjectPortfolio.Gameplay.Units;
using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.Paths
{
    internal class PathfinderGrid
    {
        private readonly PathfinderNode[] _nodes;
        private readonly int _gridSize;
        private readonly float _nodeSize;

        public float NodeSize => _nodeSize;
        public PathfinderNode[] Nodes => _nodes;

        public PathfinderGrid(IReadOnlyList<GridNode> p_systemNodes, int p_gridSize, float p_nodeSize)
        {
            _gridSize = p_gridSize;
            _nodeSize = p_nodeSize;
            
            _nodes = new PathfinderNode[p_systemNodes.Count];

            for (int i = 0; i < _nodes.Length; i++)
            {
                _nodes[i] = new PathfinderNode(p_systemNodes[i]);
            }
            
            foreach (PathfinderNode node in _nodes)
            {
                for (int i = 0; i < node.GridNode.Neighbours.Count; i++)
                {
                    int neighbourIndex = GridExtensions.PositionToIndex(node.GridNode.Neighbours[i].Position, _gridSize);
                    node.Neighbours[i] = _nodes[neighbourIndex];
                }
            }
        }
        
        public PathfinderNode GetNode(GridNode p_gridNode)
        {
            return _nodes[GridExtensions.PositionToIndex(p_gridNode.Position, _gridSize)];
        }
        
        public PathfinderNode GetNode(UnitPosition p_position)
        {
            return _nodes[GridExtensions.PositionToIndex(p_position.ToVec2(), _gridSize)];
        }
        
        public PathfinderNode GetNode(Vector2 p_worldPosition)
        {
            Vector2Int gridPosition = (p_worldPosition / _nodeSize).Floor();
            return _nodes[GridExtensions.PositionToIndex(gridPosition, _gridSize)];
        }
    }
}