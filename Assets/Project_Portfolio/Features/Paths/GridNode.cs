using System.Collections.Generic;
using ProjectPortfolio.Gameplay.Units;
using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.Paths
{
    internal class GridNode
    {
        public readonly UnitPosition Position;
        public readonly Vector2 WorldPosition;
        public readonly Bounds2D GridBounds;

        public bool IsBlocked;

        public readonly List<GridNode> Neighbours = new();
        
        public GridNode(UnitPosition p_position)
        {
            Position = p_position;
            WorldPosition = (Vector2)p_position.ToVec2() * UnitPosition.LOCAL_TO_WORLD;

            Vector2 size = Vector2.one * UnitPosition.LOCAL_TO_WORLD;
            GridBounds = new Bounds2D(WorldPosition + size / 2f, size, 0f);
        }
    }
}