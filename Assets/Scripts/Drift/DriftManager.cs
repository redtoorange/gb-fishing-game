using System;
using Boat;
using Embugerance;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Drift
{
    public class DriftManager : MonoBehaviour
    {
        public Action<int> OnDriftingSetupComplete;
        public Action OnDriftingComplete;

        private int driftHeading = 90;

        private BoatController boatController;
        private int maxDrift = 4;

        private void Start()
        {
            boatController = FindFirstObjectByType<BoatController>();
            boatController.OnBoatDriftComplete += HandleBoatDoneDrifting;
        }

        private void HandleBoatDoneDrifting()
        {
            OnDriftingComplete?.Invoke();
        }

        public void HandleSetup()
        {
            int heading = Random.Range(0, 4);
            driftHeading = heading * 90;
            if (EmbuggeranceManager.S.HasEmbuggerance(EmbuggeranceType.BadCurrent))
            {
                maxDrift = 8;
            }

            OnDriftingSetupComplete?.Invoke(driftHeading);
        }

        public void StartDrift()
        {
            boatController.HandleDrifting(driftHeading, maxDrift);
        }
    }
}