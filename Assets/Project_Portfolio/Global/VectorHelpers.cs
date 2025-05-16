using UnityEngine;

namespace ProjectPortfolio.Global
{
    public static class VectorHelpers
    {
        public static Vector2 GetXY(this Vector3 p_vector)
        {
            return new Vector2(p_vector.x, p_vector.y);
        }
        public static Vector2 GetXZ(this Vector3 p_vector)
        {
            return new Vector2(p_vector.x, p_vector.z);
        }
    }
}