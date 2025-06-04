using ProjectPortfolio.Gameplay.Units;
using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.Movement
{
    [RequireComponent(typeof(IUnitPathDriver))]
    public class UnitSimpleMovement : MonoBehaviour, IUnitMovementDriver
    {
        [SerializeField] private float _speed;

        private IUnitPathDriver _pathDriver;

        private void Awake()
        {
            _pathDriver = GetComponent<IUnitPathDriver>();
        }

        private void Update()
        {
            if (!_pathDriver.HasNextNode())
            {
                return;
            }
            
            Vector2 delta = _pathDriver.PeekNextNode() - transform.position.GetXZ();
            Vector2 direction = delta.normalized;
            
            float distanceToTarget = delta.magnitude;
            float moveDistance = _speed * Time.deltaTime;
            
            if (moveDistance >= distanceToTarget)
            {
                moveDistance = distanceToTarget;
                _pathDriver.PopNextNode();
            }

            transform.position += (direction * moveDistance).ToXZ();
        }
    }
}