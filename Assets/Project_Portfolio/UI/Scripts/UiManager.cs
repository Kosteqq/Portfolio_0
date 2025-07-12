using ProjectPortfolio.Global;
using UnityEngine;

namespace ProjectPortfolio.UI
{
    public class UiManager : MonoBehaviour
    {
        private void Awake()
        {
            GameRegistry.Instance.Register(this);
        }
    }
}