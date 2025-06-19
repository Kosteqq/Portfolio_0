using UnityEngine;

namespace ProjectPortfolio.Features.Combat
{
    public struct Ray
    {
        public Vector2 Origin;
        public Vector2 Direction;
    }

    public struct HitResult
    {
        public Vector2 Position;
        public Vector2 Normal;
        public Vector2 OutDirection;
        public float Distance;
        public bool IsBlocked;
    }
    
    public struct HitHistory
    {
        public Vector2 WorldPosition;
        public Vector2 WorldHitNormal;

        public HitHistory(in HitResult p_result)
        {
            WorldPosition = p_result.Position;
            WorldHitNormal = p_result.Normal;
        }
    }
}
