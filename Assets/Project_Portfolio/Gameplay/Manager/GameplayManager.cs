using ProjectPortfolio.Gameplay.Interaction;
using UnityEngine;

namespace ProjectPortfolio.Gameplay
{
    [RequireComponent(typeof(InteractionGameplay))]
    public class GameplayManager : MonoBehaviour
    {
        private InteractionGameplay _interactionComponent;

        private void Awake()
        {
            _interactionComponent = GetComponent<InteractionGameplay>();
            // _interactionManager.
        }
    }
}