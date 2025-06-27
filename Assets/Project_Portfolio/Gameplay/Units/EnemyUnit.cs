using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.Gameplay.Units
{
    public class EnemyUnit : Unit
    {
        private void Update()
        {
            if (!_detectionDriver.TryGetTarget(out GameObject target))
            {
                _movementDriver.Stop();
                return;
            }
            
            Vector2 targetPosition = target.transform.position.GetXZ();
            bool isAttacking = _equipmentDriver.CanAttack(targetPosition);
            
            UnitState desireState = (isAttacking && _equipmentDriver.RequiredSiegeState) 
                ? UnitState.SIEGE
                : UnitState.MOVE;

            if (!EnsureIsInState(desireState))
                return;

            if (isAttacking)
            {
                _equipmentDriver.Attack(targetPosition);
            }
            else
            {
                _equipmentDriver.CancelAttack();
            }

            if (desireState == UnitState.MOVE)
            {
                _pathDriver.SetDestination(UnitPosition.WorldToLocal(targetPosition));
            }
        }
    }
}