using ProjectPortfolio.Gameplay.Interaction;
using UnityEngine;

namespace ProjectPortfolio.Gameplay
{
    [RequireComponent(typeof(InteractionGameplayComponent))]
    public class GameplayManager : MonoBehaviour
    {
        private InteractionGameplayComponent _interactionComponent;

        private void Awake()
        {
            _interactionComponent = GetComponent<InteractionGameplayComponent>();
            // _interactionManager.
        }
    }
}