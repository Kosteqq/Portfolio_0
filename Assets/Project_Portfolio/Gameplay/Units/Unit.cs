using ProjectPortfolio.Global;
using ProjectPortfolio.Movement;
using UnityEngine;

namespace ProjectPortfolio.Gameplay.Units
{
    [RequireComponent(typeof(IUnitMovementDriver))]
    [RequireComponent(typeof(IUnitPathDriver))]
    public class Unit : MonoBehaviour
    {
        public IUnitMovementDriver MovementDriver { get; private set; }
        public IUnitPathDriver PathDriver { get; private set; }

        private void Awake()
        {
            MovementDriver = GetComponent<UnitSimpleMovement>();
            PathDriver = GetComponent<IUnitPathDriver>();
        }
    }
}