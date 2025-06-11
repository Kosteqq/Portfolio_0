using System;

namespace ProjectPortfolio.Gameplay.Units
{
    public interface IUnitEquipmentDriver
    {
        bool CanMoveUnity { get; }
        
        event Action OnChangedActivity;

        bool EnsureEquipmentEnabled();
        bool EnsureEquipmentDisabled();
    }
}