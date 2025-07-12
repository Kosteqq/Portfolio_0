using System;
using UnityEngine;

namespace ProjectPortfolio.Global
{
    public enum GameState : byte
    {
        MainMenu, Gameplay
    }

    public class GameManager : MonoBehaviour
    {
        private SceneManager _sceneManager;
        private GameState _desireState;
        
        public GameState DesireState => _desireState;
        public event Action OnChangingState;

        private void Awake()
        {
            GameRegistry.Instance.Register(this);
        }

        private void Start()
        {
            _sceneManager = GameRegistry.Instance.Get<SceneManager>();
            _desireState = _sceneManager.GetLoadedState();
        }

        public void EnterMainMenu()
        {
            _desireState = GameState.MainMenu;
            _sceneManager.LoadScene(GameState.MainMenu);
            OnChangingState?.Invoke();
        }
        
        public void EnterNewGame()
        {
            _desireState = GameState.Gameplay;
            _sceneManager.LoadScene(GameState.Gameplay);
            OnChangingState?.Invoke();
        }

        public void Quit()
        {
            
        }
    }
}