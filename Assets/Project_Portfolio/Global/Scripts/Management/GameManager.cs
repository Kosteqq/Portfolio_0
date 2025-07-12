using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectPortfolio.Global
{
    public enum GameState : byte
    {
        MainMenu, Loading, Gameplay
    }
    
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private string _mainMenuSceneName;
        [SerializeField] private string _gameplaySceneName;
        
        private AsyncOperation _loadingSceneOperatioon;
        private GameState _currentState;
        
        public float LoadingSceneProgress => _loadingSceneOperatioon?.progress ?? 1f;

        public event Action<GameState> OnStateChanged; 

        private void Awake()
        {
            GameRegistry.Instance.Register(this);
        }

        private void Start()
        {
#if UNITY_EDITOR
            _currentState = SceneManager.GetActiveScene().name == _mainMenuSceneName
                ? GameState.MainMenu
                : GameState.Gameplay;
#endif
        }

        public void EnterMainMenu()
        {
            OnStateChanged?.Invoke(GameState.Loading);
            
            _loadingSceneOperatioon = SceneManager.LoadSceneAsync(_mainMenuSceneName, LoadSceneMode.Single);
            _loadingSceneOperatioon.completed += _ =>
            {
                OnStateChanged?.Invoke(GameState.MainMenu);
            };
        }
        
        public void EnterNewGame()
        {
            OnStateChanged?.Invoke(GameState.Loading);

            _loadingSceneOperatioon = SceneManager.LoadSceneAsync(_gameplaySceneName, LoadSceneMode.Single);
            _loadingSceneOperatioon.completed += _ =>
            {
                OnStateChanged?.Invoke(GameState.Gameplay);
            };
        }

        public void Quit()
        {
            
        }
    }
}