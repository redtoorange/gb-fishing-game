using System;
using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class PauseMenuController : MonoBehaviour
    {
        public Action OnPauseMenuClosed;

        public void OnResumeClicked()
        {
            OnPauseMenuClosed?.Invoke();
        }

        public void OnSettingsClicked()
        {
        }

        public void OnQuitClicked()
        {
            GameManager.S.OnToggleGamePaused(false);
            SceneManager.LoadScene("0_MainMenu");
        }
    }
}