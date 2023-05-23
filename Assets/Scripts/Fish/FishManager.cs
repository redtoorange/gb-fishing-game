using System;
using System.Collections.Generic;
using Embugerance;
using Game;
using UnityEngine;

namespace Fish
{
    public class FishManager : MonoBehaviour
    {
        public Action OnAllFishSetupComplete;
        public Action OnAllFishMovementComplete;
        public Action OnAllFishCaught;
        public Action<int> OnFishCaught;

        private List<FishController> managedFish;
        private List<FishController> movingFish;

        private bool isDoingSetup = false;

        private int fishMovementSpeed = 4;

        private void Start()
        {
            managedFish = new List<FishController>(GetComponentsInChildren<FishController>());
            foreach (FishController fish in managedFish)
            {
                fish.OnFishCaught += HandleOnFishCaught;
                fish.OnFishDoneMoving += HandleOnFishDoneMoving;
            }
        }

        public void HandleFishSetup()
        {
            isDoingSetup = true;
            movingFish = new List<FishController>();
            for (int i = 1; i < managedFish.Count; i++)
            {
                FishController fish = managedFish[i];
                movingFish.Add(fish);
                fish.MoveFish(8);
            }

            if (EmbuggeranceManager.S.HasEmbuggerance(EmbuggeranceType.FastFish))
            {
                fishMovementSpeed = 6;
            }
        }

        public void MoveFish()
        {
            movingFish = new List<FishController>();
            foreach (FishController fish in managedFish)
            {
                movingFish.Add(fish);
                fish.MoveFish(fishMovementSpeed);
            }
        }

        public void HandleOnFishCaught(FishController whichFish)
        {
            managedFish.Remove(whichFish);
            OnFishCaught?.Invoke(5 - managedFish.Count);
            if (managedFish.Count == 0)
            {
                OnAllFishCaught?.Invoke();
            }
            else if (GameManager.S.GetCurrentPhase() == GamePhase.MOVE_FISH)
            {
                HandleOnFishDoneMoving(whichFish);
            }
        }

        private void HandleOnFishDoneMoving(FishController whichFish)
        {
            movingFish.Remove(whichFish);
            if (movingFish.Count == 0)
            {
                if (isDoingSetup)
                {
                    isDoingSetup = false;
                    OnAllFishSetupComplete?.Invoke();
                }
                else
                {
                    OnAllFishMovementComplete?.Invoke();
                }
            }
        }
    }
}