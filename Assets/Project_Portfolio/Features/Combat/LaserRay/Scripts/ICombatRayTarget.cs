namespace ProjectPortfolio.Features.Combat
{
    internal interface ICombatRayTarget
    {
        bool IsHit(in Ray p_ray, out HitResult p_hit);
    }
}