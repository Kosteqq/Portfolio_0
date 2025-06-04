using UnityEngine;

namespace ProjectPortfolio.Global
{
    static class Bounds2DExtensions
    {
        private struct Line
        {
            public Vector2 Direction;
            public Vector2 Center;
        }

        public static Vector2[] GetCorners(this Bounds2D p_bounds)
        {
            return GetSquareCorners(p_bounds.Center, p_bounds.Size, p_bounds.Rotation);
        }
        
        public static bool IsIntersectWith(this Bounds2D p_bounds, in Bounds2D p_other)
        {
            Vector2[] boundsCorners = GetSquareCorners(p_bounds.Center, p_bounds.Size, p_bounds.Rotation);
            Line boundsHorizontalNormalAxis = GetHorizontalNormalAxis(boundsCorners, p_bounds.Center);
            Line boundsVerticalNormalAxis = GetVerticalNormalAxe(boundsCorners, p_bounds.Center);

            Vector2[] otherCorners = GetSquareCorners(p_other.Center, p_other.Size, p_other.Rotation);
            Line otherHorizontalNormalAxis = GetHorizontalNormalAxis(otherCorners, p_other.Center);
            Line otherVerticalNormalAxis = GetVerticalNormalAxe(otherCorners, p_other.Center);

            bool intersecting = true;
            intersecting &= IsOverlappingOnAxis(otherCorners, boundsHorizontalNormalAxis, p_bounds.Size.y);
            intersecting &= IsOverlappingOnAxis(otherCorners, boundsVerticalNormalAxis, p_bounds.Size.x);
            intersecting &= IsOverlappingOnAxis(boundsCorners, otherHorizontalNormalAxis, p_other.Size.y);
            intersecting &= IsOverlappingOnAxis(boundsCorners, otherVerticalNormalAxis, p_other.Size.x);

            return intersecting;
        }

        private static Vector2[] GetSquareCorners(Vector2 p_center, Vector2 p_size, float p_rotationDegrees)
        {
            Vector2 half = p_size / 2f;

            var corners = new Vector2[]
            {
                new Vector2(-half.x, -half.y),
                new Vector2(half.x, -half.y),
                new Vector2(half.x, half.y),
                new Vector2(-half.x, half.y)
            };

            float radians = -p_rotationDegrees * Mathf.Deg2Rad;
            for (int i = 0; i < corners.Length; i++)
            {
                corners[i] = RotatePointY(corners[i], radians) + p_center;
            }

            return corners;
        }

        private static Vector2 RotatePointY(Vector2 p_point, float p_radians)
        {
            float cos = Mathf.Cos(p_radians);
            float sin = Mathf.Sin(p_radians);
            
            return new Vector2(
                p_point.x * cos - p_point.y * sin,
                p_point.x * sin + p_point.y * cos
            );
        }

        private static Line GetHorizontalNormalAxis(Vector2[] p_corners, Vector2 p_center)
        {
            Vector2 horizontalEdge = p_corners[1] - p_corners[0];
            return new Line
            {
                Center = p_center,
                Direction = new Vector2(-horizontalEdge.y, horizontalEdge.x).normalized,
            };
        }

        private static Line GetVerticalNormalAxe(Vector2[] p_corners, Vector2 p_center)
        {
            Vector2 verticalEdge = p_corners[1] - p_corners[2];
            return new Line
            {
                Center = p_center,
                Direction = new Vector2(-verticalEdge.y, verticalEdge.x).normalized,
            };
        }

        private static bool IsOverlappingOnAxis(Vector2[] p_corners, Line p_otherNormalAxis, float p_otherSize)
        {
            (float min, float max) = GetProjected(p_corners, p_otherNormalAxis);
            float halfSize = p_otherSize / 2f;
            
            return min < 0 && max > 0 || Mathf.Abs(min) < halfSize || Mathf.Abs(max) < halfSize;
        }

        private static (float min, float max) GetProjected(Vector2[] p_rectCorners, Line p_axis)
        {
            float min = float.MaxValue;
            float max = float.MinValue;
            
            foreach (Vector2 corner in p_rectCorners)
            {
                Vector2 axisRelative = corner - p_axis.Center;
                float value = Vector2.Dot(axisRelative, p_axis.Direction);

                min = Mathf.Min(min, value);
                max = Mathf.Max(max, value);
            }

            return (min, max);
        }
    }
}