using System.Collections.Generic;
using UnityEngine;

namespace ProjectPortfolio.Gameplay.Units
{
    public interface IUnitPathDriver
    {
        IReadOnlyList<Vector2> PathNodes { get; }

        void SetTarget(Vector2 p_worldPosition);
        bool HasNextNode();
        Vector2 PeekNextNode();
        Vector2 PopNextNode();
    }
}