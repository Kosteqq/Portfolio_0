using System;
using NUnit.Framework;
using ProjectPortfolio.Gameplay.Units;
using ProjectPortfolio.Paths;
using UnityEngine;

namespace ProjectPortfolio.Features.Combat
{
    public class ForceFieldUnitEquipment : MonoBehaviour, IUnitEquipmentDriver
    {
        [SerializeField] private float _activateTime;
        
        [Space]
        [SerializeField] private GameObject _head;
        [SerializeField] private float _disabledHeadHeight;
        [SerializeField] private float _enabledHeadHeight;
        [SerializeField] private float _workingHeadRotationSpeed;
        [SerializeField] private GameObject _forceField;

        private bool _targetActivity;
        private float _activateTimer;

        public bool IsActive => _targetActivity && _activateTimer.IsApprox(_activateTime);
        public bool IsChanningActivity => _targetActivity ? !_activateTimer.IsApprox(_activateTime) : !_activateTimer.IsApprox(0f);
        public bool CanMoveUnity => !IsActive && !IsChanningActivity;

        public event Action OnChangedActivity;

        private void Update()
        {
            if (IsChanningActivity)
            {
                TickChanningActivity();

                if (IsChanningActivity)
                    OnChangedActivity?.Invoke();
            }

            if (IsActive)
            {
                _head.transform.eulerAngles += new Vector3(0f, _workingHeadRotationSpeed * Time.deltaTime, 0f);
            }

            if (_forceField.activeSelf != IsActive)
            {
                _forceField.SetActive(IsActive);
            }
        }

        public bool EnsureEquipmentEnabled()
        {
            _targetActivity = true;
            return IsActive;
        }

        public bool EnsureEquipmentDisabled()
        {
            _targetActivity = false;
            return _activateTimer == 0f;
        }

        private void TickChanningActivity()
        {
            if (_head.transform.localEulerAngles.y % 90f > 0f)
            {
                _head.transform.eulerAngles += new Vector3(
                    0f,
                    Mathf.Min(90f - _head.transform.localEulerAngles.y % 90f, _workingHeadRotationSpeed * Time.deltaTime),
                    0f);
                return;
            }

            float direction = _targetActivity ? 1f : -1f;
            _activateTimer = Mathf.Clamp(_activateTimer + Time.deltaTime * direction, 0f, _activateTime);

            _head.transform.localPosition = new Vector3(
                _head.transform.localPosition.x,
                Mathf.Lerp(_disabledHeadHeight, _enabledHeadHeight, _activateTimer / _activateTime),
                _head.transform.localPosition.z);
        }
    }
}