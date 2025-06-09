using System.Collections.Generic;
using UnityEngine;

namespace ProjectPortfolio.Gameplay.Units
{
    public interface IUnitPathDriver
    {
        IReadOnlyList<Vector2> PathNodes { get; }

        void SetTarget(UnitPosition p_position);
        bool HasNextNode();
        Vector2 PeekNextNode();
        Vector2 PopNextNode();
    }
}