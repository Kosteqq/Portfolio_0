using UnityEngine;

namespace ProjectPortfolio.Gameplay.Units
{
    public interface IUnitDetectionDriver : IUnitComponent
    {
        bool TryGetTarget(out GameObject p_target);
    }
}