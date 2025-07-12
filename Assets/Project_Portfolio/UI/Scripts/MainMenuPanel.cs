using ProjectPortfolio.Global;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectPortfolio.UI
{
    public class MainMenuPanel : MonoBehaviour
    {
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _exitButton;

        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameRegistry.Instance.Get<GameManager>();
            _gameManager.OnChangingState += HandleDesireChangingState;
            HandleDesireChangingState();
            
            _newGameButton.onClick.AddListener(_gameManager.EnterNewGame);
            _exitButton.onClick.AddListener(_gameManager.Quit);
        }

        private void HandleDesireChangingState()
        {
            gameObject.SetActive(_gameManager.DesireState == GameState.MainMenu);
        }
    }
}