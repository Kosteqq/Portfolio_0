using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.UI
{
    public class GameplayPanel : MonoBehaviour
    {
        private GameManager _gameManager;

        private void Start()
        {
            _gameManager = GameRegistry.Instance.Get<GameManager>();
            _gameManager.OnChangingState += HandleDesireChangingState;
            HandleDesireChangingState();
        }

        private void HandleDesireChangingState()
        {
            gameObject.SetActive(_gameManager.DesireState == GameState.Gameplay);
        }
    }
}