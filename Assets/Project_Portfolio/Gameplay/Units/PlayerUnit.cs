namespace ProjectPortfolio.Gameplay.Units
{
    public class PlayerUnit : Unit
    {
        private UnitState _desiredMode = UnitState.MOVE;

        private void Update()
        {
            if (_desiredMode == UnitState.MOVE)
            {
                TickMoveMode();
                return;
            }
            
            TickSiegeMode();
        }

        private void TickMoveMode()
        {
            if (!EnsureIsInState(UnitState.MOVE))
                return;
            
            // Move to point
        }

        private void TickSiegeMode()
        {
            if (!EnsureIsInState(UnitState.SIEGE))
                return;
            
            // attack?
        }

        public void ToggleSiegeMode()
        {
            if (_desiredMode == UnitState.MOVE)
            {
                _desiredMode = UnitState.SIEGE;
                _movementDriver.Stop();
                return;
            }
            
            _desiredMode = UnitState.MOVE;
        }

        #region Movement

        public UnitPosition GetPosition()
        {
            return UnitPosition.WorldToLocal(transform.position);
        }

        public UnitPosition GetMoveDestination()
        {
            return _pathDriver.DestinationPosition;
        }
        
        public void MoveTo(UnitPosition p_position)
        {
            _pathDriver.SetDestination(p_position);
        }
        
        public bool CanMove()
        {
            return _desiredMode == UnitState.MOVE && !_chassisDriver.InStationaryMode;
        }
        
        public bool CanMoveTo(UnitPosition p_position)
        {
            return _pathDriver.CanSetTarget(p_position);
        }

        #endregion
    }
}