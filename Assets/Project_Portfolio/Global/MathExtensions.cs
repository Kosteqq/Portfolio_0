using UnityEngine;

namespace ProjectPortfolio.Paths
{
    public static class MathExtensions
    {
        public static bool IsApprox(this float p_value, float p_other)
        {
            return Mathf.Approximately(p_value, p_other);
        }
    }
}