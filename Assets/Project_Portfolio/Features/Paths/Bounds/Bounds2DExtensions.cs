using System;
using UnityEngine;

namespace ProjectPortfolio.Global
{
    public partial struct Bounds2D
    {
        private struct Line
        {
            public Vector2 Direction;
            public Vector2 Center;
        }

        public readonly bool IsIntersectsRay(Vector2 p_rayOrigin, Vector2 p_rayDirection,
            out Vector2 p_closestIntersect, out Vector2 p_intersectNormal)
        {
            Vector2[] corners = GetCorners();

            p_closestIntersect = default;
            p_intersectNormal = default;
            float closestIntersectDist = float.MaxValue;

            for (int i = 0; i < corners.Length; i++)
            {
                if (!IsLineIntersect(p_rayOrigin, p_rayDirection,
                        corners[i], corners[(i + 1) % corners.Length],
                        out Vector2 intersect))
                {
                    continue;
                }

                float dist = Vector2.Distance(p_rayOrigin, intersect);
                
                if (dist < closestIntersectDist)
                {
                    closestIntersectDist = dist;
                    var line = corners[(i + 1) % corners.Length] - corners[i];
                    p_intersectNormal = new Vector2(line.y, -line.x);
                    p_closestIntersect = intersect;
                }
            }

            return closestIntersectDist < float.MaxValue;
        }
        
        private static bool IsLineIntersect(Vector2 p_rayOrigin, Vector2 p_rayDirection,
            Vector2 p_lineStart, Vector2 p_lineEnd,
            out Vector2 p_intersect)
        {
            p_intersect = default;
            
            // Line vector and ray vector
            Vector2 lineVector = p_lineEnd - p_lineStart;
            Vector2 rayToLineStart = p_lineStart - p_rayOrigin;

            // Calculate the determinant
            float det = p_rayDirection.x * lineVector.y - p_rayDirection.y * lineVector.x;

            if (Mathf.Abs(det) < Mathf.Epsilon)
            {
                // The ray and the line are parallel
                return false;
            }

            // Solve for t and s
            float t = (rayToLineStart.x * p_rayDirection.y - rayToLineStart.y * p_rayDirection.x) / det;
            float s = (rayToLineStart.x * lineVector.y - rayToLineStart.y * lineVector.x) / det;

            if (t >= 0 && t <= 1 && s >= 0)
            {
                // Calculate intersection point
                p_intersect = p_lineStart + t * lineVector;
                return true;
            }

            return false;


            /*Vector2 rayToLine = p_rayOrigin - p_lineStart;
            Vector2 lineDelta = p_lineEnd - p_lineStart;
            Vector2 rayPerpendicular = new Vector2(-p_rayDirection.y, p_rayDirection.x);

            float dot = Vector2.Dot(lineDelta, rayPerpendicular);
            bool areParallel = Mathf.Abs(dot) < Mathf.Epsilon;

            if (areParallel)
            {
                return false;
            }

            // float lineParameter = Vector2.Dot(rayToLine.normalized, rayPerpendicular.normalized) / dot;
            float t1 = lineDelta.Cross(rayToLine) / dot;
            float t2 = Vector2.Dot(rayToLine, rayPerpendicular) / dot;

            Debug.Log($"{dot} {t1} {t2}");
            if (t1 >= 0.0 && (t2 >= 0.0 && t2 <= 1.0))
            {
                p_intersect = p_rayOrigin + p_rayDirection * t1;
                return true;
            }
            return false;*/

            /*bool isIntersecting = lineParameter > 0 && lineParameter < 1;

            if (!isIntersecting)
            {
                return false;
            }

            float t = Vector2.Dot(rayToLine, lineDelta) / dot;
            bool isBehind = t < 0;

            if (isBehind)
            {
                return false;
            }

            p_intersect = p_rayOrigin + p_rayDirection * t;
            return true;*/
        }
    
        public readonly float DistanceToPoint(Vector2 p_relativePoint)
        {
            Vector2[] corners = GetCorners();
            float minDistance = float.MaxValue;

            for (int i = 0; i < corners.Length; i++)
            {
                Vector2 cornerA = corners[i];
                Vector2 cornerB = corners[(i + 1) % corners.Length];

                float distance = DistanceToLine(p_relativePoint, cornerA, cornerB);
                minDistance = Mathf.Min(minDistance, distance);
            }

            return minDistance;
        }

        private readonly float DistanceToLine(Vector2 p_lineStart, Vector2 p_lineEnd, Vector2 p_point)
        {
            Vector2 line = p_lineEnd - p_lineStart;
            Vector2 lineDirection = line.normalized;
            float lineLenght = line.magnitude;
            
            float projectedDistance = Vector2.Dot(lineDirection, p_point - p_lineStart);

            if (projectedDistance < 0f)
            {
                return Vector2.Distance(p_point, p_lineStart);
            }
            if (projectedDistance > lineLenght)
            {
                return Vector2.Distance(p_point, p_lineEnd);
            }
            
            Vector2 projectedPoint = p_lineStart + lineDirection * projectedDistance;
            
            return Vector2.Distance(projectedPoint, p_point);
        }

        public readonly Vector2[] GetCorners()
        {
            return GetSquareCorners(Center, Size, Rotation);
        }

        public readonly bool Contains(Vector2 p_point)
        {
            Vector2 localPoint = Center - p_point;

            float sin = Mathf.Sin(Rotation * Mathf.Deg2Rad);
            float cos = Mathf.Cos(Rotation * Mathf.Deg2Rad);
            localPoint = new Vector2(
                localPoint.x * cos - localPoint.y * sin,
                localPoint.x * sin + localPoint.y * cos);

            Vector2 halfSize = Size / 2f;
            return localPoint.x >= -halfSize.x
                   && localPoint.y >= -halfSize.y
                   && localPoint.x <= halfSize.x
                   && localPoint.y <= halfSize.y;
        }
        
        public readonly bool IsIntersectWith(in Bounds2D p_other)
        {
            Vector2[] boundsCorners = GetSquareCorners(Center, Size, Rotation);
            Line boundsHorizontalNormalAxis = GetHorizontalNormalAxis(boundsCorners, Center);
            Line boundsVerticalNormalAxis = GetVerticalNormalAxe(boundsCorners, Center);

            Vector2[] otherCorners = GetSquareCorners(p_other.Center, p_other.Size, p_other.Rotation);
            Line otherHorizontalNormalAxis = GetHorizontalNormalAxis(otherCorners, p_other.Center);
            Line otherVerticalNormalAxis = GetVerticalNormalAxe(otherCorners, p_other.Center);

            bool intersecting = true;
            intersecting &= IsOverlappingOnAxis(otherCorners, boundsHorizontalNormalAxis, Size.y);
            intersecting &= IsOverlappingOnAxis(otherCorners, boundsVerticalNormalAxis, Size.x);
            intersecting &= IsOverlappingOnAxis(boundsCorners, otherHorizontalNormalAxis, p_other.Size.y);
            intersecting &= IsOverlappingOnAxis(boundsCorners, otherVerticalNormalAxis, p_other.Size.x);

            return intersecting;
        }

        private readonly Vector2[] GetSquareCorners(Vector2 p_center, Vector2 p_size, float p_rotationDegrees)
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

        private readonly Vector2 RotatePointY(Vector2 p_point, float p_radians)
        {
            float cos = Mathf.Cos(p_radians);
            float sin = Mathf.Sin(p_radians);
            
            return new Vector2(
                p_point.x * cos - p_point.y * sin,
                p_point.x * sin + p_point.y * cos
            );
        }

        private readonly Line GetHorizontalNormalAxis(Vector2[] p_corners, Vector2 p_center)
        {
            Vector2 horizontalEdge = p_corners[1] - p_corners[0];
            return new Line
            {
                Center = p_center,
                Direction = new Vector2(-horizontalEdge.y, horizontalEdge.x).normalized,
            };
        }

        private readonly Line GetVerticalNormalAxe(Vector2[] p_corners, Vector2 p_center)
        {
            Vector2 verticalEdge = p_corners[1] - p_corners[2];
            return new Line
            {
                Center = p_center,
                Direction = new Vector2(-verticalEdge.y, verticalEdge.x).normalized,
            };
        }

        private readonly bool IsOverlappingOnAxis(Vector2[] p_corners, Line p_otherNormalAxis, float p_otherSize)
        {
            (float min, float max) = GetProjected(p_corners, p_otherNormalAxis);
            float halfSize = p_otherSize / 2f;
            
            return min < 0 && max > 0 || Mathf.Abs(min) < halfSize || Mathf.Abs(max) < halfSize;
        }

        private readonly (float min, float max) GetProjected(Vector2[] p_rectCorners, Line p_axis)
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