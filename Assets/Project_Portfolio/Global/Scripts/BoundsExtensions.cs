using UnityEngine;

namespace ProjectPortfolio.Global
{
    public static class BoundsExtensions
    {
        /// <summary>
        /// Calculates the Bounds corners coordinates transformed into world space.
        /// </summary>
        public static Vector3[] GetWorldCorners(this Bounds p_bounds, Transform p_transform)
        {
            Vector3 center = p_bounds.center;
            Vector3 extents = p_bounds.extents;

            var corners =  new Vector3[] {
                new Vector3( center.x + extents.x, center.y + extents.y, center.z + extents.z ),
                new Vector3( center.x + extents.x, center.y + extents.y, center.z - extents.z ),
                new Vector3( center.x + extents.x, center.y - extents.y, center.z + extents.z ),
                new Vector3( center.x + extents.x, center.y - extents.y, center.z - extents.z ),
                new Vector3( center.x - extents.x, center.y + extents.y, center.z + extents.z ),
                new Vector3( center.x - extents.x, center.y + extents.y, center.z - extents.z ),
                new Vector3( center.x - extents.x, center.y - extents.y, center.z + extents.z ),
                new Vector3( center.x - extents.x, center.y - extents.y, center.z - extents.z ),
            };

            for (var i = 0; i < corners.Length; i++)
            {
                corners[i] = p_transform.TransformPoint(corners[i]);
            }

            return corners;
        }
    }
}