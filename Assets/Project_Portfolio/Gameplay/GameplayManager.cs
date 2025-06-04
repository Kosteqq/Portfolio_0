using ProjectPortfolio.Gameplay.Interaction;
using UnityEngine;

namespace ProjectPortfolio.Gameplay
{
    [RequireComponent(typeof(InteractionManager))]
    public class GameplayManager : MonoBehaviour
    {
        private InteractionManager _interactionManager;

        private void Awake()
        {
            _interactionManager = GetComponent<InteractionManager>();
            // _interactionManager.
        }
    }
}