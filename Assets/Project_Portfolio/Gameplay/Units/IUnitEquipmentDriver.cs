using UnityEngine;

namespace ProjectPortfolio.Gameplay.Units
{
    public interface IUnitEquipmentDriver : IUnitComponent
    {
        bool RequiredSiegeState { get; }
        
        bool CanAttack(Vector2 p_targetPosition);
        void Attack(Vector2 p_targetPosition);
        void CancelAttack();
    }
}