using System.Collections.Generic;
using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.Paths
{
    internal class GridNode
    {
        public readonly Vector2Int Position;
        public readonly Vector2 WorldPosition;
        public readonly Bounds2D WorldBounds;

        public bool IsBlocked;

        public readonly List<GridNode> Neighbours = new();
        
        public GridNode(Vector2Int p_position, Vector2 p_worldPosition, Vector2 p_worldSize)
        {
            Position = p_position;
            WorldPosition = p_worldPosition;
            WorldBounds = new Bounds2D(p_worldPosition + p_worldSize / 2f, p_worldSize, 0f);
        }
    }
}