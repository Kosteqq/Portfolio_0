using ProjectPortfolio.Global;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectPortfolio.Gameplay.UI
{
    public class LoadingPanel : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _contentGroup;
        [SerializeField] private Image _progressImage;
        [SerializeField] private float _fadeOutTime;
        [SerializeField] private float _blockInteractionThreshold = 0.15f;
        
        private GameManager _gameManager;
        
        private void Start()
        {
            _gameManager = GameRegistry.Instance.Get<GameManager>();
            _gameManager.OnStateChanged += HandleChangedState;
        }

        private void Update()
        {
            _progressImage.fillAmount
                = Mathf.Lerp(_progressImage.fillAmount, _gameManager.LoadingSceneProgress, Time.unscaledDeltaTime);

            _contentGroup.blocksRaycasts = _contentGroup.alpha > _blockInteractionThreshold;
            
            bool shouldUnload = _gameManager.LoadingSceneProgress >= 1f && _progressImage.fillAmount > 0.8f;
            
            if (shouldUnload)
            {
                _contentGroup.alpha -= 1f / _fadeOutTime * Time.unscaledDeltaTime;
            }
            
            if (_contentGroup.alpha <= 0f)
            {
                gameObject.SetActive(false);
            }
        }

        private void HandleChangedState(GameState p_newState)
        {
            if (p_newState == GameState.Loading)
            {
                gameObject.SetActive(true);
                _contentGroup.alpha = 1f;
                _progressImage.fillAmount = 0f;
            }
        }
    }
}