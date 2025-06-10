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

        public UnitPosition GetPosition()
        {
            return UnitPosition.WorldToLocal(transform.position);
        }

        public UnitPosition GetMoveDestination()
        {
            return _pathDriver.DestinationPosition;
        }
        
        public void MoveTo(UnitPosition p_position)
        {
            _pathDriver.SetDestination(p_position);
        }
        
        public bool CanMoveTo(UnitPosition p_position)
        {
            return _pathDriver.CanSetTarget(p_position);
        }
    }
}