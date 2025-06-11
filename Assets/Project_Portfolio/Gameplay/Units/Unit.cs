using System.Collections;
using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.Gameplay.Units
{
    public enum UnitState
    {
        MOVE, SIEGE
    }
    
    [RequireComponent(typeof(IUnitMovementDriver))]
    [RequireComponent(typeof(IUnitPathDriver))]
    [RequireComponent(typeof(IUnitEquipmentDriver))]
    [RequireComponent(typeof(IUnitChassisDriver))]
    public class Unit : MonoBehaviour
    {
        private IUnitPathDriver _pathDriver;
        private IUnitMovementDriver _movementDriver;
        private IUnitChassisDriver _chassisDriver;
        private IUnitEquipmentDriver _equipmentDriver;

        private UnitState _desiredMode;

        private void Awake()
        {
            _pathDriver = GetComponent<IUnitPathDriver>();
            _movementDriver = GetComponent<IUnitMovementDriver>();
            _chassisDriver = GetComponent<IUnitChassisDriver>();
            _equipmentDriver = GetComponent<IUnitEquipmentDriver>();
            
            _movementDriver.OnArrived += MovementArrivedHandler;
            _equipmentDriver.OnChangedActivity += EquipmentActivityChanged;
        }

        public void ToggleMode()
        {
            if (_desiredMode == UnitState.SIEGE)
            {
                SetMoveMode();
            }
            else
            {
                SetSiegeMode();
            }
        }
        
        public void SetMoveMode()
        {
            _desiredMode = UnitState.MOVE;
            
            if (!_equipmentDriver.EnsureEquipmentDisabled())
            {
                return;
            }

            if (!_chassisDriver.EnsureStationaryDisabled())
            {
                return;
            }
        }

        public void SetSiegeMode()
        {
            _desiredMode = UnitState.SIEGE;
            
            if (!_movementDriver.EnsureStopped())
            {
                return;
            }

            if (!_chassisDriver.EnsureStationaryEnabled())
            {
                return;
            }

            _equipmentDriver.EnsureEquipmentEnabled();
        }

        private void EquipmentActivityChanged()
        {
            if (_desiredMode == UnitState.SIEGE)
            {
                SetSiegeMode();
                return;
            }
            if (_desiredMode == UnitState.MOVE)
            {
                SetMoveMode();
                return;
            }
        }

        private void MovementArrivedHandler()
        {
            if (_desiredMode == UnitState.SIEGE)
            {
                SetSiegeMode();
            }
        }

        #region Movement

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
        
        public bool CanMove()
        {
            return _desiredMode == UnitState.MOVE && _equipmentDriver.CanMoveUnity && !_chassisDriver.InStationaryMode;
        }
        
        public bool CanMoveTo(UnitPosition p_position)
        {
            return _pathDriver.CanSetTarget(p_position);
        }

        #endregion

        #region Siege Mode

        private IEnumerator X()
        {
            yield break;
        }

        #endregion
    }
}