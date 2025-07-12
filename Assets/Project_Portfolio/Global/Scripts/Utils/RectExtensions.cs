using System.Collections.Generic;
using UnityEngine;

namespace ProjectPortfolio.Global
{
    public static class RectExtensions
    {
        public static Rect WorldPointsIntoScreenRect(in IReadOnlyCollection<Vector3> p_worldSpacePoints, Camera p_camera)
        {
            if (!Asserts.IsTrue(p_worldSpacePoints.Count > 1, "Need at least 2 points to create a rect"))
            {
                return new Rect();
            }
            
            float maxX = float.MinValue;
            float minX = float.MaxValue;
            float maxY = float.MinValue;
            float minY = float.MaxValue;
            
            foreach (Vector3 worldPoint in p_worldSpacePoints)
            {
                Vector3 screenSpacePoint = p_camera.WorldToScreenPoint(worldPoint);
                maxX = Mathf.Max(maxX, screenSpacePoint.x);
                minX = Mathf.Min(minX, screenSpacePoint.x);
                maxY = Mathf.Max(maxY, screenSpacePoint.y);
                minY = Mathf.Min(minY, screenSpacePoint.y);
            }
            
            return Rect.MinMaxRect(minX, minY, maxX, maxY);
        }
    }
}