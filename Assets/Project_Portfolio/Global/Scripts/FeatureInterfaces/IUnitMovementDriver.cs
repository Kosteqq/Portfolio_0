using ProjectPortfolio.Gameplay.Units;

namespace ProjectPortfolio.Global
{
    public interface IUnitMovementDriver : IUnitComponent
    {
        void Stop();
    }
}