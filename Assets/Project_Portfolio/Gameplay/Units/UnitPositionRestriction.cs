using System;

namespace ProjectPortfolio.Gameplay.Units
{
    [Flags]
    public enum UnitPositionRestriction : byte
    {
        None = 0,
        NoStaticObstacle = 1 << 0,
        NoDynamicObstacle = 1 << 1,
        Unoccupied = NoStaticObstacle | NoDynamicObstacle,
        RemainUnoccupied = 1 << 2,
    }
}