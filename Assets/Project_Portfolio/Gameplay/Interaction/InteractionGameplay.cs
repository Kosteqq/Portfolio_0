using System.Collections.Generic;
using System.Linq;
using ProjectPortfolio.Gameplay.Units;
using ProjectPortfolio.Global;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectPortfolio.Gameplay.Interaction
{
    public class InteractionGameplay : MonoBehaviour
    {
        const int MAX_SEARCH_RANGE = 5;
        
        private InteractionInput _interactionInput;
        
        internal readonly List<PlayerUnit> SelectedUnits = new();
        internal InteractionInput Input => _interactionInput;
        
        private void Awake()
        {
            _interactionInput = new InteractionInput();
            _interactionInput.Enable();
            _interactionInput.UnitsManagement.SetPosition.performed += SetUnitsPosition;
            _interactionInput.UnitsManagement.ToggleModes.performed += ToggleModes;
        }

        private void ToggleModes(InputAction.CallbackContext p_obj)
        {
            foreach (PlayerUnit unit in SelectedUnits)
            {
                unit.ToggleMode();
            }
        }

        private void SetUnitsPosition(InputAction.CallbackContext p_context)
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (plane.Raycast(ray, out float enterDistance))
            {
                UnitPosition targetUnitPosition = UnitPosition.WorldToLocal(ray.GetPoint(enterDistance));

                Vector2 averagePosition = Vector2.zero;
                foreach (PlayerUnit unit in SelectedUnits)
                {
                    averagePosition += unit.transform.position.GetXZ();
                }
                averagePosition /= SelectedUnits.Count;

                IOrderedEnumerable<PlayerUnit> unitsInOrder = SelectedUnits
                    .OrderBy(unit => Vector2.Distance(unit.transform.position.GetXZ(), averagePosition));

                var excludedPositions = new List<UnitPosition>();
                
                foreach (PlayerUnit unit in unitsInOrder)
                {
                    if (!unit.CanMove())
                        continue;
                    
                    UnitPosition? target = GetClosestAvailableTarget(unit, targetUnitPosition, excludedPositions);
                        
                    if (target.HasValue)
                    {
                        unit.MoveTo(target.Value);
                        excludedPositions.Add(target.Value);
                    }
                }
            }
        }

        private UnitPosition? GetClosestAvailableTarget(PlayerUnit p_unit, UnitPosition p_targetPosition, List<UnitPosition> p_excludedPositions)
        {
            int searchRange = Mathf.Min(
                (p_unit.GetPosition().ToVec2() - p_targetPosition.ToVec2()).sqrMagnitude,
                (p_unit.GetMoveDestination().ToVec2() - p_targetPosition.ToVec2()).sqrMagnitude,
                MAX_SEARCH_RANGE);
            
            for (int range = 0; range < searchRange; range++)
            {
                foreach (UnitPosition position in GetClosestPositions(p_targetPosition, range))
                {
                    if (p_unit.CanMoveTo(position) && !p_excludedPositions.Contains(position))
                    {
                        return position;
                    }
                }
            }

            return null;
        }

        private List<UnitPosition> GetClosestPositions(UnitPosition p_unitPosition, int p_range)
        {
            Vector2Int original = p_unitPosition.ToVec2();
            var positions = new List<UnitPosition>(p_range * p_range);

            if (p_range == 0)
            {
                if (UnitPosition.IsValid(original.x, original.y))
                    positions.Add(new UnitPosition(original.x, original.y));
                return positions;
            }
            
            // bottom
            for (int x = 0; x <= p_range; x++)
            {
                if (UnitPosition.IsValid(original.x + x, original.y - p_range))
                    positions.Add(new UnitPosition(original.x + x, original.y - p_range));
                if (UnitPosition.IsValid(original.x - x, original.y - p_range))
                    positions.Add(new UnitPosition(original.x - x, original.y - p_range));
            }

            // left & right
            for (int y = -p_range + 1; y <= p_range - 1; y++)
            {
                if (UnitPosition.IsValid(original.x - p_range, original.y + y))
                    positions.Add(new UnitPosition(original.x - p_range, original.y + y));
                if (UnitPosition.IsValid(original.x + p_range, original.y + y))
                    positions.Add(new UnitPosition(original.x + p_range, original.y + y));
            }
            
            // top
            for (int x = p_range; x >= 0; x--)
            {
                if (UnitPosition.IsValid(original.x + x, original.y + p_range))
                    positions.Add(new UnitPosition(original.x + x, original.y + p_range));
                if (UnitPosition.IsValid(original.x - x, original.y + p_range))
                    positions.Add(new UnitPosition(original.x - x, original.y + p_range));
            }

            return positions;
        }
    }
}