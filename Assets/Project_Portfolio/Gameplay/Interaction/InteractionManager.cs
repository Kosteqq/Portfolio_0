using ProjectPortfolio.Gameplay.Units;
using ProjectPortfolio.Global;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectPortfolio.Gameplay.Interaction
{
    public class InteractionManager : MonoBehaviour
    {
        private InteractionInput _interactionInput;
        
        private void Awake()
        {
            _interactionInput = new InteractionInput();
            _interactionInput.Enable();
            _interactionInput.UnitsManagement.SetPosition.performed += SetUnitsPosition;
        }

        private void SetUnitsPosition(InputAction.CallbackContext p_context)
        {
            Unit[] units = FindObjectsByType<Unit>(FindObjectsSortMode.None);

            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (plane.Raycast(ray, out float enterDistance))
            {
                Vector2 worldPos = ray.GetPoint(enterDistance).GetXZ();

                foreach (Unit unit in units)
                {
                    unit.PathDriver.SetTarget(worldPos);
                }
            }
        }
    }
}