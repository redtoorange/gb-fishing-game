using System;
using TMPro;
using UnityEngine;

namespace Embugerance
{
    public class EmbuggeranceDescriptionPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text descriptionDisplay;

        private void Start()
        {
            EmbuggeranceToggle.OnEmbuggeranceHovered += HandleEmbuggeranceHovered;
            EmbuggeranceToggle.OnEmbuggeranceUnHovered += HandleEmbuggeranceUnHovered;
            gameObject.SetActive(false);
        }

        private void HandleEmbuggeranceUnHovered()
        {
            gameObject.SetActive(false);
        }

        private void HandleEmbuggeranceHovered(string description)
        {
            descriptionDisplay.text = description;
            gameObject.SetActive(true);
        }
    }
}