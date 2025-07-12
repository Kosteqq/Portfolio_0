using ProjectPortfolio.Global;
using ProjectPortfolio.Global.Input;
using UnityEngine;

namespace ProjectPortfolio.UI
{
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _contentGroup;
        [SerializeField] private float _fadeOutTime;
        [SerializeField] private float _blockInteractionThreshold = 0.15f;
        
        private InputManager _inputManager;
        private SceneManager _sceneManager;
        
        private void Start()
        {
            _inputManager = GameRegistry.Instance.Get<InputManager>();
            _sceneManager = GameRegistry.Instance.Get<SceneManager>();
            _sceneManager.OnSceneLoading += HandleLoadingScene;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            _contentGroup.blocksRaycasts = _contentGroup.alpha > _blockInteractionThreshold;
            
            if (!_sceneManager.IsLoading)
            {
                TickFadeOut();
            }
            
            if (_contentGroup.alpha <= 0f)
            {
                gameObject.SetActive(false);
                _inputManager.Enable();
            }
        }

        private void TickFadeOut()
        {
            _contentGroup.alpha -= 1f / _fadeOutTime * Time.unscaledDeltaTime;
        }

        private void HandleLoadingScene()
        {
            if (_sceneManager.IsLoading)
            {
                gameObject.SetActive(true);
                _contentGroup.alpha = 1f;
                
                _inputManager.Disable();
            }
        }
    }
}