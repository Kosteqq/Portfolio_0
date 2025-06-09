using System.Collections.Generic;
using ProjectPortfolio.Gameplay.Units;
using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.Paths
{
    internal class GridNode
    {
        public readonly Vector2Int Position;
        public readonly Vector2 WorldPosition;
        public readonly GridBounds GridBounds;

        public bool IsBlocked;

        public readonly List<GridNode> Neighbours = new();
        
        public GridNode(Vector2Int p_position, Vector2 p_worldPosition, Vector2 p_worldSize)
        {
            Position = p_position;
            WorldPosition = p_worldPosition;
            GridBounds = new GridBounds(p_worldPosition + p_worldSize / 2f, p_worldSize, 0f);
        }
    }
}