using ProjectPortfolio.Gameplay.Units;
using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.Features.Combat
{
    public class LaserCannonUnitEquipment : MonoBehaviour, IUnitEquipmentDriver
    {
        [SerializeField] private float _range;
        [SerializeField] private Transform _raySource;
        [SerializeField] private Transform _turret;
        [SerializeField] private float _turretRotationSpeed = 25f;
        [SerializeField] private float _shootAngleError = 5f;

        private Vector2? _targetPosition;
        private float _cooldown;

        public bool RequiredSiegeState => true;

        private void Update()
        {
            _cooldown -= Time.deltaTime;

            RotateTurretTowardsTarget(_targetPosition.GetValueOrDefault(Vector2.up));
        }

        private void RotateTurretTowardsTarget(Vector2 p_targetPosition)
        {
            Vector2 directionToTarget = (p_targetPosition - transform.position.GetXZ()).normalized;
            
            float targetAngle = Mathf.Atan2(-directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            targetAngle += 90f;
            targetAngle %= 360f;
            float currentAngle = _turret.eulerAngles.y;

            float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
            float rotationStep = Mathf.Min(Mathf.Abs(angleDifference), _turretRotationSpeed * Time.deltaTime) * Mathf.Sign(angleDifference);

            _turret.eulerAngles = new Vector3(_turret.eulerAngles.x, currentAngle + rotationStep, _turret.eulerAngles.z);
        }

        public bool CanAttack(Vector2 p_targetPosition)
        {
            return Vector2.Distance(p_targetPosition, transform.position.GetXZ()) < _range;
        }

        public void Attack(Vector2 p_targetPosition)
        {
            _targetPosition = p_targetPosition;
            Vector2 currentDirection = _raySource.transform.forward.GetXZ().normalized;
            Vector2 directionToTarget = (p_targetPosition - transform.position.GetXZ()).normalized;

            float angle = Vector2.Angle(currentDirection, directionToTarget);
            bool isTurretFacingTarget = angle <= _shootAngleError;

            if (_cooldown <= 0f && isTurretFacingTarget)
            {
                _cooldown = 4f;
                
                // HACKME
                FindAnyObjectByType<CombatLaserController>().ShootRay(new Ray
                {
                    Origin = _raySource.transform.position.GetXZ(),
                    Direction = directionToTarget,
                });
            }
        }

        public void CancelAttack()
        {
            _targetPosition = null;
        }
    }
}