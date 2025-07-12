using UnityEngine;

namespace ProjectPortfolio.Global
{
    public static class VectorExtensions
    {
        public static Vector2 GetXY(this Vector3 p_vector)
        {
            return new Vector2(p_vector.x, p_vector.y);
        }
        
        public static Vector2 GetXZ(this Vector3 p_vector)
        {
            return new Vector2(p_vector.x, p_vector.z);
        }

        public static Vector3 ToXZ(this Vector2 p_vector, float p_height = 0f)
        {
            return new Vector3(p_vector.x, p_height, p_vector.y);
        }
        
        public static Vector2Int Floor(this Vector2 p_vector)
        {
            return new Vector2Int(Mathf.FloorToInt(p_vector.x), Mathf.FloorToInt(p_vector.y));
        }
        
        public static Vector2Int Round(this Vector2 p_vector)
        {
            return new Vector2Int(Mathf.RoundToInt(p_vector.x), Mathf.RoundToInt(p_vector.y));
        }
        
        public static Vector2Int Ceil(this Vector2 p_vector)
        {
            return new Vector2Int(Mathf.CeilToInt(p_vector.x), Mathf.CeilToInt(p_vector.y));
        }
        
        public static Vector3Int Floor(this Vector3 p_vector)
        {
            return new Vector3Int(Mathf.FloorToInt(p_vector.x), Mathf.FloorToInt(p_vector.y), Mathf.FloorToInt(p_vector.z));
        }
        
        public static Vector3Int Round(this Vector3 p_vector)
        {
            return new Vector3Int(Mathf.RoundToInt(p_vector.x), Mathf.RoundToInt(p_vector.y), Mathf.RoundToInt(p_vector.z));
        }
        
        public static Vector3Int Ceil(this Vector3 p_vector)
        {
            return new Vector3Int(Mathf.CeilToInt(p_vector.x), Mathf.CeilToInt(p_vector.y), Mathf.CeilToInt(p_vector.z));
        }

        #region Abs

        public static Vector2 Abs(this Vector2 p_vector)
        {
            return new Vector2(Mathf.Abs(p_vector.x), Mathf.Abs(p_vector.y));
        }

        public static Vector2Int Abs(this Vector2Int p_vector)
        {
            return new Vector2Int(Mathf.Abs(p_vector.x), Mathf.Abs(p_vector.y));
        }

        public static Vector3 Abs(this Vector3 p_vector)
        {
            return new Vector3(Mathf.Abs(p_vector.x), Mathf.Abs(p_vector.y), Mathf.Abs(p_vector.z));
        }

        public static Vector3Int Abs(this Vector3Int p_vector)
        {
            return new Vector3Int(Mathf.Abs(p_vector.x), Mathf.Abs(p_vector.y), Mathf.Abs(p_vector.z));
        }

        #endregion

        #region Cross

        public static float Cross(this Vector2 p_vector, Vector2 p_other)
        {
            return p_vector.x * p_other.x - p_vector.y * p_other.y;
        }

        #endregion
    }
}