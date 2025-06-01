using System.Collections.Generic;
using UnityEngine;

namespace ProjectPortfolio.Paths
{
    internal class GridNode
    {
        public Vector2Int Position;
        public Vector2 WorldPosition;

        public readonly List<GridNode> Neighbours = new();

        public GridNode(Vector2Int p_position, Vector2 p_worldPosition)
        {
            Position = p_position;
            WorldPosition = p_worldPosition;
        }
    }
}