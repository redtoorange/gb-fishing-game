using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialUIController : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text tutorialTextOutput;
    [SerializeField] private List<string> tutorialItems;

    private int tutorialIndex = 0;

    private void Start()
    {
        tutorialTextOutput.text = tutorialItems[tutorialIndex];
    }

    public void OnBackClicked()
    {
        tutorialIndex -= 1;
        tutorialTextOutput.text = tutorialItems[tutorialIndex];
        continueButton.gameObject.SetActive(true);

        if (tutorialIndex == 0)
        {
            backButton.gameObject.SetActive(false);
        }
    }
    
    public void OnContinueClicked()
    {
        tutorialIndex += 1;
        tutorialTextOutput.text = tutorialItems[tutorialIndex];
        backButton.gameObject.SetActive(true);

        if (tutorialIndex == tutorialItems.Count - 1)
        {
            continueButton.gameObject.SetActive(false);
        }
    }

    public void OnSkipClicked()
    {
        SceneManager.LoadScene("MainGame");
    }
}