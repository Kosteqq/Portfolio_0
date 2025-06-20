using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.Gameplay.Units
{
    public enum UnitState : byte
    {
        MOVE,
        SIEGE
    }

    [RequireComponent(typeof(IUnitMovementDriver))]
    [RequireComponent(typeof(IUnitPathDriver))]
    [RequireComponent(typeof(IUnitEquipmentDriver))]
    [RequireComponent(typeof(IUnitChassisDriver))]
    [RequireComponent(typeof(IUnitDetectionDriver))]
    public abstract class Unit : MonoBehaviour
    {
        protected IUnitPathDriver _pathDriver { get; private set; }
        protected IUnitMovementDriver _movementDriver { get; private set; }
        protected IUnitChassisDriver _chassisDriver { get; private set; }
        protected IUnitEquipmentDriver _equipmentDriver { get; private set; }
        protected IUnitDetectionDriver _detectionDriver { get; private set; }

        protected virtual void Awake()
        {
            _pathDriver = GetComponent<IUnitPathDriver>();
            _movementDriver = GetComponent<IUnitMovementDriver>();
            _chassisDriver = GetComponent<IUnitChassisDriver>();
            _equipmentDriver = GetComponent<IUnitEquipmentDriver>();
            _detectionDriver = GetComponent<IUnitDetectionDriver>();
        }

        protected bool EnsureIsInState(UnitState p_desireState)
        {
            bool isInState = true;

            if (!_pathDriver.EnsureIsInState(p_desireState))
                isInState = false;
            if (!_movementDriver.EnsureIsInState(p_desireState))
                isInState = false;
            if (!_chassisDriver.EnsureIsInState(p_desireState))
                isInState = false;
            if (!_equipmentDriver.EnsureIsInState(p_desireState))
                isInState = false;
            if (!_detectionDriver.EnsureIsInState(p_desireState))
                isInState = false;
            
            return isInState;
        }
    }
}