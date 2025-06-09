using ProjectPortfolio.Global;
using ProjectPortfolio.Movement;
using UnityEngine;

namespace ProjectPortfolio.Gameplay.Units
{
    [RequireComponent(typeof(IUnitMovementDriver))]
    [RequireComponent(typeof(IUnitPathDriver))]
    public class Unit : MonoBehaviour
    {
        private IUnitMovementDriver _movementDriver;
        private IUnitPathDriver _pathDriver;

        private void Awake()
        {
            _movementDriver = GetComponent<UnitSimpleMovement>();
            _pathDriver = GetComponent<IUnitPathDriver>();
        }
        
        public void MoveTo(UnitPosition p_position)
        {
            _pathDriver.SetTarget(p_position);
        }
        }
    }
}