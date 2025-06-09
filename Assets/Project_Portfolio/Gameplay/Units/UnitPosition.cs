using UnityEngine;
using ProjectPortfolio.Global;

namespace ProjectPortfolio.Gameplay.Units
{
    public struct UnitPosition
    {
        private const string REQUREMENT_POSITIVE_MSG = "Unit position have to be positive";
        public const float LOCAL_TO_WORLD = 5f;
        
        public ushort X;
        public ushort Y;

        public UnitPosition(int p_x, int p_y)
        {
            if (!Asserts.IsTrue(p_x > 0, REQUREMENT_POSITIVE_MSG))
                p_x = 0;
            if (!Asserts.IsTrue(p_y > 0, REQUREMENT_POSITIVE_MSG))
                p_y = 0;

            X = (ushort)p_x;
            Y = (ushort)p_y;
        }
        
        public Vector2Int ToVec2()
        {
            return new Vector2Int(X, Y);
        }

        public static UnitPosition WorldToLocal(Vector3 p_worldPosition)
        {
            return WorldToLocal(p_worldPosition.GetXZ());
        }

        public static UnitPosition WorldToLocal(Vector2 p_worldPosition)
        {
            if (!Asserts.IsTrue(p_worldPosition.x >= 0 && p_worldPosition.y >= 0, REQUREMENT_POSITIVE_MSG))
                p_worldPosition = Vector2.Max(p_worldPosition, Vector2.zero);
            
            return new UnitPosition
            {
                X = (ushort)Mathf.FloorToInt(p_worldPosition.x / LOCAL_TO_WORLD),
                Y = (ushort)Mathf.FloorToInt(p_worldPosition.y / LOCAL_TO_WORLD),
            };
        }
    }
}