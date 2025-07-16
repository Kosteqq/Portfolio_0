using System.Linq;
using ProjectPortfolio.Global;
using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

namespace ProjectPortfolio.Gameplay
{
    internal class SceneRegistry : Registry
    {
        public Scene SceneHandle { get; }
        
        private SceneRegistry(Scene p_sceneHandle)
        {
            SceneHandle = p_sceneHandle;
        }
        
        public static SceneRegistry Get(MonoBehaviour p_mono)
        {
            return Get(p_mono.gameObject.scene);
        }
        
        public static SceneRegistry Get(GameObject p_gameObject)
        {
            return Get(p_gameObject.scene);
        }
        
        public static SceneRegistry Get(Scene p_sceneHandle)
        {
            SceneRegistry registry = GameRegistry.Instance
                .GetAll<SceneRegistry>()
                .FirstOrDefault(registry => registry.SceneHandle == p_sceneHandle);

            if (registry == null)
            {
                registry = new SceneRegistry(p_sceneHandle);
                GameRegistry.Instance.Register(registry);
                
                var container = new GameObject("| Scene Container |");
                SceneManager.MoveGameObjectToScene(container, p_sceneHandle);

                container.AddComponent<SceneRegistryContainer>().Setup(registry);
            }

            return registry;
        }
    }
}