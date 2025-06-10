using System.Collections.Generic;
using UnityEngine;

namespace ProjectPortfolio.Gameplay.Units
{
    public interface IUnitPathDriver
    {
        IReadOnlyList<Vector2> PathNodes { get; }
        UnitPosition DestinationPosition { get; }

        void SetDestination(UnitPosition p_position);
        bool HasNextNode();
        Vector2 PeekNextNode();
        void PopNextNode();
        bool CanSetTarget(UnitPosition p_position);
    }
}