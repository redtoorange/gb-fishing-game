using System.Collections.Generic;
using UnityEngine;

namespace DiceRoller
{
    [CreateAssetMenu(fileName = "DiceRollerConfig", menuName = "DiceRollerConfig", order = 0)]
    public class DiceRollerConfig : ScriptableObject
    {
        [Header("Tween")]
        public float fromValue = 0.0f;
        public float toValue = 100.0f;
        public float tweenTime = 1.0f;
        public LeanTweenType easeType = LeanTweenType.easeInOutCubic;

        [Header("Randomizer")]
        public float cutOffValue = 1000.0f;

        [Header("Display")]
        public float displayTime = 1.0f;
        public List<Sprite> diceSprites;
    }
}