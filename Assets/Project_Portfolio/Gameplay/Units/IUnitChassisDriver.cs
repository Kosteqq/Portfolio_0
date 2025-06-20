namespace ProjectPortfolio.Gameplay.Units
{
    public interface IUnitChassisDriver : IUnitComponent
    {
        bool InStationaryMode { get; }
    }
}