using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fish
{
    public class FishMovementArrow : MonoBehaviour
    {
        public Action<float> OnSpinComplete;


        [SerializeField] private Transform arrowPivot;
        [SerializeField] private float rotateSpeed = 1.0f;

        private static bool hasBeenSeeded = false;

        private bool isSpinning;
        private float lastSpin;

        public void ShowArrow()
        {
            gameObject.SetActive(true);
        }

        public void HideArrow()
        {
            gameObject.SetActive(false);
        }

        public void SpinArrow(float startingAngle)
        {
            if (isSpinning) return;

            arrowPivot.rotation = Quaternion.Euler(0, 0, startingAngle);
            ShowArrow();
            isSpinning = true;
            lastSpin = GetRandomHeading();

            LeanTween.value(
                    gameObject,
                    PerformArrowSpinTween,
                    startingAngle,
                    lastSpin,
                    rotateSpeed
                )
                .setEase(LeanTweenType.easeInOutCubic)
                .setOnComplete(HandleArrowSpinTweenComplete);
        }

        private void HandleArrowSpinTweenComplete()
        {
            HideArrow();
            isSpinning = false;

            OnSpinComplete?.Invoke(lastSpin);
        }

        private void PerformArrowSpinTween(float value)
        {
            arrowPivot.rotation = Quaternion.Euler(0, 0, value);
        }

        private float GetRandomHeading()
        {
            if (!hasBeenSeeded)
            {
                Random.InitState((int)Time.time);
                hasBeenSeeded = true;
            }

            float targetRotation = Random.Range(0.0f, 360.0f);
            float spinTime = Random.Range(3, 8) * 360.0f;
            float positiveOrNegative = Random.Range(0.0f, 1.0f > 0.5 ? 1.0f : -1.0f);

            return (targetRotation + spinTime) * positiveOrNegative;
        }
    }
}