using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DiceRoller
{
    public class DiceRollController : MonoBehaviour
    {
        public Action<int> OnDiceRollCompleted;

        [SerializeField] private DiceRollerConfig diceRollerConfig;
        [SerializeField] private SpriteRenderer diceDisplay;

        private int minRoll = 1;
        private int maxRoll = 8;
        private int lastChangeValue = 0;


        public void RollDice(int minimum, int maximum)
        {
            if (LeanTween.isTweening(gameObject))
            {
                Debug.LogError("Attempted to tween DiceRollController more than once");
                return;
            }

            minRoll = minimum;
            maxRoll = maximum;
            diceDisplay.enabled = true;
            lastChangeValue = 0;

            LeanTween.value(
                    gameObject,
                    PerformDiceAnimation,
                    diceRollerConfig.fromValue,
                    diceRollerConfig.toValue,
                    diceRollerConfig.tweenTime
                ).setEase(diceRollerConfig.easeType)
                .setOnComplete(HandleDiceRollTweenComplete);
        }

        private void PerformDiceAnimation(float value)
        {
            int flooredValue = Mathf.FloorToInt(value);
            if (flooredValue % diceRollerConfig.cutOffValue == 0 && flooredValue > lastChangeValue)
            {
                lastChangeValue = flooredValue;
                diceDisplay.sprite = diceRollerConfig.diceSprites[Random.Range(minRoll, maxRoll + 1)];
                if (diceDisplay.sprite == null)
                {
                    Debug.Log("Null Sprite");
                }
            }
        }

        private void HideDisplay()
        {
            diceDisplay.enabled = false;
        }

        private void HandleDiceRollTweenComplete()
        {
            int roll = Random.Range(minRoll, maxRoll + 1);
            diceDisplay.sprite = diceRollerConfig.diceSprites[roll];
            Invoke(nameof(HideDisplay), diceRollerConfig.displayTime);
            OnDiceRollCompleted?.Invoke(roll);
        }
    }
}