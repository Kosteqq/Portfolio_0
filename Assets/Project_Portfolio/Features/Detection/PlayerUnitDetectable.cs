using ProjectPortfolio.Gameplay.Units;
using UnityEngine;

namespace ProjectPortfolio.Features.Detection
{
    public class PlayerUnitDetectable : MonoBehaviour, IUnitDetectionDriver
    {
        public bool TryGetTarget(out GameObject p_target)
        {
            // HACKME: Temp solution just for testing
            var playerUnit = FindAnyObjectByType<PlayerUnit>();
            p_target = playerUnit.gameObject;
            return true;
        }
    }
}