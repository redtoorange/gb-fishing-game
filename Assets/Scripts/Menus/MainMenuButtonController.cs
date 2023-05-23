using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class MainMenuButtonController : MonoBehaviour
    {
        public void OnStartGameButtonPressed()
        {
            SceneManager.LoadScene("TutorialSection");
        }
        
        public void OnFreePlayButtonPressed()
        {
            SceneManager.LoadScene("EmbuggeranceMenu");
        }

        public void OnQuitGameButtonPressed()
        {
            Application.Quit();
        }
    }
}