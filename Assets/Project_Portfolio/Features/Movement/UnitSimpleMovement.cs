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
        private bool _canMove;
        private bool _isMoving;

        private void Awake()
        {
            _pathDriver = GetComponent<IUnitPathDriver>();
        }

        private void Update()
        {
            if (!_pathDriver.HasNextNode())
            {
                if (_isMoving)
                {
                    _isMoving = false;
                }
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

            _isMoving = true;
            transform.position += (direction * moveDistance).ToXZ();
        }

        public bool EnsureIsInState(UnitState p_desireState)
        {
            if (p_desireState == UnitState.MOVE)
            {
                _canMove = true;
                return true;
            }

            if (p_desireState == UnitState.SIEGE)
            {
                Stop();
                return true;
            }
            
            return true;
        }

        public void Stop()
        {
            _pathDriver.ClearPath();
            _canMove = false;
        }
    }
}