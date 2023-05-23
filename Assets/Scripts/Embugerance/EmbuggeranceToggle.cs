using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Embugerance
{
    public class EmbuggeranceToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public static Action<EmbuggeranceType, bool> OnEmbuggeranceToggled;
        public static Action<string> OnEmbuggeranceHovered;
        public static Action OnEmbuggeranceUnHovered;
        
        [SerializeField] private EmbuggeranceType embuggeranceType;
        [SerializeField][Multiline] private string description;
        
        private Toggle toggle;

        private void Start()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(HandleValueChanged);
        }

        private void HandleValueChanged(bool value)
        {
            OnEmbuggeranceToggled?.Invoke(embuggeranceType, value);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEmbuggeranceHovered?.Invoke(description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnEmbuggeranceUnHovered?.Invoke();
        }
    }
}