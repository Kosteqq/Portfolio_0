using System;

namespace ProjectPortfolio.Global
{
    public interface IUnitMovementDriver
    {
        event Action OnArrived;
        
        bool EnsureStopped();
    }
}