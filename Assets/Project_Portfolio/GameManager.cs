using UnityEngine;

namespace ProjectPortfolio
{
    public class GameManager : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            var manager = Resources.Load<GameManager>("Game_Manager");
            DontDestroyOnLoad(manager);
        }
    }
}