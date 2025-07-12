using ProjectPortfolio.Global.Input.Internal;
using UnityEngine;
using UnityEngine.InputSystem.UI;

namespace ProjectPortfolio.Global.Input
{
    [RequireComponent(typeof(InputSystemUIInputModule))]
    public class InputManager : MonoBehaviour
    {
        private GameInputActions _inputActions;

        public GameInputActions.GameplayActions GameplayMap => _inputActions.Gameplay;
        public GameInputActions.UIActions UIActions => _inputActions.UI;

        private void Awake()
        {
            GameRegistry.Instance.Register(this);
            
            _inputActions = new GameInputActions();
            
            var inputModule = GetComponent<InputSystemUIInputModule>();
            inputModule.actionsAsset = _inputActions.asset;

            Enable();
        }

        public void Enable()
        {
            _inputActions.Enable();
        }

        public void Disable()
        {
            _inputActions.Disable();
        }
    }
}