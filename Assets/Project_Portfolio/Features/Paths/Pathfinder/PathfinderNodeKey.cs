using System;

namespace ProjectPortfolio.Paths
{
    internal readonly struct PathfinderNodeKey : IEquatable<PathfinderNodeKey>
    {
        public readonly float Key1;
        public readonly float Key2;

        public PathfinderNodeKey(float p_key1, float p_key2)
        {
            Key1 = p_key1;
            Key2 = p_key2;
        }
            
        public static bool operator <(PathfinderNodeKey a, PathfinderNodeKey b)
        {
            return a.Key1 < b.Key1 || (a.Key1.IsApprox(b.Key1) && a.Key2 < b.Key2);
        }
            
        public static bool operator >(PathfinderNodeKey a, PathfinderNodeKey b)
        {
            return a.Key1 > b.Key1 || (a.Key1.IsApprox(b.Key1) && a.Key2 > b.Key2);
        }
        public static bool operator <=(PathfinderNodeKey a, PathfinderNodeKey b)
        {
            return a.Key1 <= b.Key1 || (a.Key1.IsApprox(b.Key1) && a.Key2 <= b.Key2);
        }
        public static bool operator >=(PathfinderNodeKey a, PathfinderNodeKey b)
        {
            return a.Key1 >= b.Key1 || (a.Key1.IsApprox(b.Key1) && a.Key2 >= b.Key2);
        }
            
        public static bool operator ==(PathfinderNodeKey a, PathfinderNodeKey b)
        {
            return a.Key1.IsApprox(b.Key1) && a.Key2.IsApprox(b.Key2);
        }
            
        public static bool operator !=(PathfinderNodeKey a, PathfinderNodeKey b)
        {
            return !(a == b);
        }

        public override bool Equals(object p_object)
        {
            return p_object is PathfinderNodeKey otherKey && this == otherKey;
        }

        public bool Equals(PathfinderNodeKey p_other)
        {
            return this == p_other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key1, Key2);
        }
    }
}