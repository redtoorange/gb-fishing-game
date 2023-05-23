using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class EndGameMenuController : MonoBehaviour
    {
        public void OnPlayAgainButtonPressed()
        {
            SceneManager.LoadScene("MainGame");
        }

        public void OnMainMenuButtonPressed()
        {
            SceneManager.LoadScene("0_MainMenu");
        }
    }
}