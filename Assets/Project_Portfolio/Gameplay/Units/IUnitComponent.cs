namespace ProjectPortfolio.Gameplay.Units
{
    public interface IUnitComponent
    {
        bool EnsureIsInState(UnitState p_desireState) => true;
    }
}