using ProjectPortfolio.Global;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectPortfolio.Gameplay.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _exitButton;

        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameRegistry.Instance.Get<GameManager>();
            _gameManager.OnStateChanged += HandleChangedState;
            
            _newGameButton.onClick.AddListener(_gameManager.EnterNewGame);
            _exitButton.onClick.AddListener(_gameManager.Quit);
        }

        private void HandleChangedState(GameState p_newState)
        {
            gameObject.SetActive(p_newState == GameState.MainMenu);
        }
    }
}