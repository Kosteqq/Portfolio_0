using System.Collections.Generic;
using ProjectPortfolio.Gameplay.Units;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace ProjectPortfolio.Gameplay.Interaction
{
    public class InteractionSelection : MonoBehaviour
    {
        [SerializeField] private Image _selectionImage;
        
        private InteractionGameplay _interaction;
        
        private bool _isSelecting;
        private Vector2 _startSelecting;
        private Vector2 _endSelecting;

        private void Start()
        {
            _interaction = FindAnyObjectByType<InteractionGameplay>();
            _interaction.Input.UnitsManagement.Selecting.started += StartSelecting;
            _interaction.Input.UnitsManagement.Selecting.canceled += FinishSelecting;
            
            _selectionImage.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!_isSelecting)
                return;
            
            _endSelecting = Mouse.current.position.value;

            Rect selectionRect = GetSelectionRect();
            var trans = (RectTransform)_selectionImage.transform;
            trans.anchoredPosition = selectionRect.min;
            trans.sizeDelta = selectionRect.size;
        }

        private void StartSelecting(InputAction.CallbackContext p_context)
        {
            _isSelecting = true;
            _selectionImage.gameObject.SetActive(true);
            _startSelecting = Mouse.current.position.value;
        }

        private Rect GetSelectionRect()
        {
            Vector2 min = Vector2.Min(_startSelecting, _endSelecting);
            Vector2 max = Vector2.Max(_startSelecting, _endSelecting);
            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        private void FinishSelecting(InputAction.CallbackContext p_context)
        {
            _isSelecting = false;
            _selectionImage.gameObject.SetActive(false);

            Rect selectionRect = GetSelectionRect();
            
            PlayerUnit[] units = FindObjectsByType<PlayerUnit>(FindObjectsSortMode.None);
            _interaction.SelectedUnits.Clear();

            foreach (PlayerUnit unit in units)
            {
                var bounds = unit.GetComponent<InteractionSelectionBounds>();
                var selection = unit.GetComponentInChildren<DecalProjector>(true);

                selection.gameObject.SetActive(false);
                if (bounds.IsInSelection(selectionRect))
                {
                    selection.gameObject.SetActive(true);
                    _interaction.SelectedUnits.Add(unit);
                }
            }
        }
    }
}