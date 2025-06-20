using ProjectPortfolio.Gameplay.Units;
using UnityEngine;

namespace ProjectPortfolio.Features.Detection
{
    public class PlayerUnitDetectable : MonoBehaviour, IUnitDetectionDriver
    {
        public bool TryGetTarget(out GameObject p_target)
        {
            p_target = null;
            return false;
        }
    }
}