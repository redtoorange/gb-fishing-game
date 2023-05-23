using System;
using Drift;
using Embugerance;
using Fish;
using UnityEngine;

namespace Setup
{
    public class SetupManager : MonoBehaviour
    {
        public Action OnSetupComplete;

        private bool setupInProgress;

        private FishManager fishManager;
        private RockManager rockManager;
        private DriftManager driftManager;

        private void Start()
        {
            fishManager = FindFirstObjectByType<FishManager>();
            fishManager.OnAllFishSetupComplete += HandleOnFishSetupComplete;

            rockManager = FindFirstObjectByType<RockManager>();
            driftManager = FindFirstObjectByType<DriftManager>();
        }


        public void StartSetup()
        {
            if (!setupInProgress)
            {
                setupInProgress = true;
                if (EmbuggeranceManager.S.HasEmbuggerance(EmbuggeranceType.Rocks))
                {
                    rockManager.HandleSetup();
                }

                fishManager.HandleFishSetup();
            }
        }

        private void HandleOnFishSetupComplete()
        {
            driftManager.HandleSetup();

            setupInProgress = false;
            OnSetupComplete?.Invoke();
        }
    }
}