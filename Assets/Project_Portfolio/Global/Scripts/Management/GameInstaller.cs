using UnityEngine;

namespace ProjectPortfolio.Global
{
    public class GameInstaller
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            GameRegistry.Initialize();
            
            var corePrefab = Resources.Load<GameObject>("Game_Core");
            Object.DontDestroyOnLoad(Object.Instantiate(corePrefab));
        }
    }
}