namespace ProjectPortfolio.Gameplay.Units
{
    public interface IUnitChassisDriver
    {
        bool InStationaryMode { get; }
        
        bool EnsureStationaryDisabled();
        bool EnsureStationaryEnabled();
    }
}