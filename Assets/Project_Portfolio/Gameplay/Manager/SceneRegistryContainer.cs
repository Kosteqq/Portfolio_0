using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.Gameplay
{
    internal class SceneRegistryContainer : MonoBehaviour
    {
        private SceneRegistry _registry;

        internal void Setup(SceneRegistry p_sceneRegistry)
        {
            _registry = p_sceneRegistry;
        }

        private void OnDestroy()
        {
            GameRegistry.Instance.Deregister(_registry);
        }
    }
}