using System;
using ProjectPortfolio.Gameplay.Units;
using UnityEngine;

namespace ProjectPortfolio.Movement
{
    public class UnitSimpleChassis : MonoBehaviour, IUnitChassisDriver
    {
        [SerializeField] private float _distanceThreshold = 0.01f;
        private Vector3 _prevPosition;
        
        public bool InStationaryMode => false;

        private void Start()
        {
            _prevPosition = transform.position;
        }

        private void Update()
        {
            Vector3 delta = _prevPosition - transform.position;
            _prevPosition = transform.position;
            
            if (delta.magnitude > _distanceThreshold)
            {
                transform.rotation = Quaternion.LookRotation(delta.normalized, Vector3.up);
            }
        }

        public bool EnsureIsInState(UnitState p_desireState)
        {
            return true;
        }
    }
}