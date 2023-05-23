using UnityEngine;
using UnityEngine.SceneManagement;

namespace Embugerance
{
    public class EmbuggeranceMenu : MonoBehaviour
    {
        private EmbuggeranceManager embuggeranceManager;

        private void Start()
        {
            embuggeranceManager = FindFirstObjectByType<EmbuggeranceManager>();
            EmbuggeranceToggle.OnEmbuggeranceToggled += HandleEmbuggeranceToggled;
        }

        private void HandleEmbuggeranceToggled(EmbuggeranceType which, bool isEnabled)
        {
            if (isEnabled)
            {
                embuggeranceManager.AddEmbuggerance(which);
            }
            else
            {
                embuggeranceManager.RemoveEmbuggerance(which);
            }
        }

        public void OnStartClicked()
        {
            SceneManager.LoadScene("MainGame");
        }
    }
}