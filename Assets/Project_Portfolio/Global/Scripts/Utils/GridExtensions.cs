using UnityEngine;

namespace ProjectPortfolio.Global
{
    public static class GridExtensions
    {
        public static int PositionToIndex(Vector2Int p_position, int p_gridSize)
        {
            return p_position.y * p_gridSize + p_position.x;
        }
    }
}