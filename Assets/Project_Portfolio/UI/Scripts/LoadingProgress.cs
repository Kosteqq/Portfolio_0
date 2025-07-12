using ProjectPortfolio.Global;
using UnityEngine;
using UnityEngine.Events;

namespace ProjectPortfolio.UI
{
    public class LoadingProgress : MonoBehaviour
    {
        [SerializeField] private UnityEvent<float> _updateProgress;
        
        private SceneManager _sceneManager;
        private float _displayedProgress;
        
        private void Start()
        {
            _sceneManager = GameRegistry.Instance.Get<SceneManager>();
            _sceneManager.OnSceneLoading += StartLoading;
        }

        private void Update()
        {
            // Smooth update progress
            _displayedProgress += Time.unscaledDeltaTime;
            _displayedProgress = Mathf.Min(_displayedProgress, _sceneManager.LoadingProgress);
            
            _updateProgress.Invoke(_displayedProgress);
        }

        private void StartLoading()
        {
            _displayedProgress = 0f;
            _updateProgress.Invoke(_displayedProgress);
        }
    }
}