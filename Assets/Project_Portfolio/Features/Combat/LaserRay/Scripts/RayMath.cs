using UnityEngine;

namespace ProjectPortfolio.Features.Combat
{
    internal static class RayMath
    {
        public static bool TryReflect(Vector2 p_direction, Vector2 p_normal, out Vector2 p_outDirection)
        {
            float angle = (1f + Vector2.Dot(p_normal, p_direction)) * 90f;

            if (angle < 17f)
            {
                p_outDirection = default;
                return false;
            }

            p_outDirection = Vector2.Reflect(p_direction, p_normal);
            return true;
        }
    }
}