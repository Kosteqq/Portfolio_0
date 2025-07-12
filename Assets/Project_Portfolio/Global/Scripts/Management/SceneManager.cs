using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectPortfolio.Global
{
    public class SceneManager : MonoBehaviour
    {
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private string _gameplaySceneName;

        private AsyncOperation _loadingOperation;

        public float LoadingProgress => _loadingOperation?.progress ?? 1f;
        public bool IsLoading => _loadingOperation != null;

        public event Action OnSceneLoading; 
        public event Action OnSceneLoaded; 

        private void Awake()
        {
            GameRegistry.Instance.Register(this);
        }

        internal void LoadScene(GameState p_gameState)
        {
            GameState currentSceneState = GetLoadedState();

            if (currentSceneState == p_gameState)
            {
                return;
            }

            string sceneName = p_gameState == GameState.MainMenu ? _mainMenuSceneName : _gameplaySceneName;
            _loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            Asserts.IsNotNull(_loadingOperation);
            
            _loadingOperation.completed += _ =>
            {
                _loadingOperation = null;
                OnSceneLoaded?.Invoke();
            };
            
            OnSceneLoading?.Invoke();
        }

        public GameState GetLoadedState()
        {
            return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == _mainMenuSceneName
                ? GameState.MainMenu
                : GameState.Gameplay;
        }
    }
}