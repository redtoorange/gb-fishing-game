using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonController : MonoBehaviour
{
    public void OnStartGameButtonPressed()
    {
        SceneManager.LoadScene("TutorialSection");
    }

    public void OnQuitGameButtonPressed()
    {
        Application.Quit();
    }
}