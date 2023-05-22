using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameUIController : MonoBehaviour
    {
        [SerializeField] private GameObject helpTextPanel;
        [SerializeField] private TMP_Text helpTextLabel;
        [SerializeField] private TMP_Text fishCountText;
        [SerializeField] private Image driftArrowImage;
        [SerializeField] private TMP_Text phaseText;


        public void ShowHelpText(string text)
        {
            helpTextLabel.text = text;
            helpTextPanel.SetActive(true);
        }

        public void HideHelpText()
        {
            helpTextPanel.SetActive(false);
        }

        public void SetFishCaughtCount(int newCount)
        {
            fishCountText.text = newCount.ToString();
        }

        public void SetDriftArrowHeading(int heading)
        {
            LeanTween.value(driftArrowImage.gameObject,
                (newValue) => { driftArrowImage.transform.rotation = Quaternion.Euler(0, 0, newValue); }, 0.0f,
                heading,
                1.0f
            ).setEase(LeanTweenType.easeInOutCubic);
        }

        public void SetPhaseText(string text)
        {
            phaseText.text = text;
        }
    }
}