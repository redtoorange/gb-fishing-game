using Game;
using Menus;
using UnityEngine;

namespace UI
{
    public class GamePauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject gameHud;
        [SerializeField] private GameObject pauseMenu;
        private PauseMenuController pauseMenuController;

        [SerializeField] private bool gamePaused = false;

        private void Start()
        {
            pauseMenuController = pauseMenu.GetComponent<PauseMenuController>();
            pauseMenuController.OnPauseMenuClosed += UnPauseGame;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!gamePaused)
                {
                    PauseGame();
                }
                else
                {
                    UnPauseGame();
                }
            }
        }


        private void PauseGame()
        {
            gamePaused = true;
            GameManager.S.OnToggleGamePaused(true);
            gameHud.SetActive(false);
            pauseMenu.SetActive(true);
        }

        private void UnPauseGame()
        {
            gameHud.SetActive(true);
            pauseMenu.SetActive(false);
            GameManager.S.OnToggleGamePaused(false);
            gamePaused = false;
        }

        public bool IsGamePaused() => gamePaused;
    }
}