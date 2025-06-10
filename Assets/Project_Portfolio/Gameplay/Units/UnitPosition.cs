using System;
using UnityEngine;
using ProjectPortfolio.Global;

namespace ProjectPortfolio.Gameplay.Units
{
    public struct UnitPosition : IEquatable<UnitPosition>
    {
        private const string REQUREMENT_POSITIVE_MSG = "Unit position have to be positive";
        public const float LOCAL_TO_WORLD = 3f;
        
        public ushort X;
        public ushort Y;

        public UnitPosition(int p_x, int p_y)
        {
            if (!Asserts.IsTrue(p_x >= 0, REQUREMENT_POSITIVE_MSG))
                p_x = 0;
            if (!Asserts.IsTrue(p_y >= 0, REQUREMENT_POSITIVE_MSG))
                p_y = 0;

            X = (ushort)p_x;
            Y = (ushort)p_y;
        }

        public static bool IsValid(int p_x, int p_y)
        {
            return p_x >= 0 && p_y >= 0;
        }
        
        public readonly Vector2Int ToVec2()
        {
            return new Vector2Int(X, Y);
        }
        
        public readonly Vector2 ToGlobal()
        {
            return new Vector2(X, Y) * LOCAL_TO_WORLD;
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

        public static bool operator ==(UnitPosition a, UnitPosition b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(UnitPosition a, UnitPosition b)
        {
            return !(a == b);
        }

        public bool Equals(UnitPosition p_other)
        {
            return X == p_other.X && Y == p_other.Y;
        }

        public override bool Equals(object p_object)
        {
            return p_object is UnitPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}