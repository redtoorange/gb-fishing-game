using Boat;
using Cinemachine;
using Drift;
using Fish;
using Setup;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager S;

        private void Awake()
        {
            if (S == null)
            {
                S = this;
            }
            else
            {
                Debug.Log("Detected an existing GameManager, cleaning up new one.");
                Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }

        [SerializeField] private CinemachineVirtualCamera centerCamera;
        [SerializeField] private CinemachineVirtualCamera boatCamera;
        [SerializeField] private GamePhase currentGamePhase = GamePhase.SETUP;

        public GamePhase GetCurrentPhase() => currentGamePhase;

        private SetupManager setupManager;
        private BoatController boatController;
        private FishManager fishManager;
        private DriftManager driftManager;
        private GameUIController gameUIController;

        private void Start()
        {
            gameUIController = FindFirstObjectByType<GameUIController>();

            setupManager = FindFirstObjectByType<SetupManager>();
            setupManager.OnSetupComplete += HandleOnSetupComplete;

            boatController = FindFirstObjectByType<BoatController>();
            boatController.OnBoatRotateComplete += HandleOnBoatRotateComplete;
            boatController.OnBoatMoveComplete += HandleOnBoatMoveComplete;
            boatController.OnBoatCrash += HandleOnBoatCrash;

            fishManager = FindFirstObjectByType<FishManager>();
            fishManager.OnFishCaught += HandleOnFishCaught;
            fishManager.OnAllFishCaught += HandleOnAllFishCaught;
            fishManager.OnAllFishMovementComplete += HandleOnAllFishMovementComplete;

            driftManager = FindFirstObjectByType<DriftManager>();
            driftManager.OnDriftingSetupComplete += HandleDriftSetupComplete;
            driftManager.OnDriftingComplete += HandleOnDriftingComplete;

            gameUIController.ShowHelpText("Press Space to Start");
            gameUIController.SetFishCaughtCount(0);
        }

        private void HandleDriftSetupComplete(int newHeading)
        {
            gameUIController.SetDriftArrowHeading(newHeading);
        }

        private void HandleOnFishCaught(int howMany)
        {
            gameUIController.SetFishCaughtCount(howMany);
        }

        private void Update()
        {
            if (currentGamePhase == GamePhase.SETUP)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    gameUIController.HideHelpText();
                    setupManager.StartSetup();
                }
            }
        }

        private void HandleOnSetupComplete()
        {
            centerCamera.enabled = false;
            boatCamera.enabled = true;
            currentGamePhase = GamePhase.ROTATE_BOAT;
            gameUIController.ShowHelpText("Rotate Boat with A and D, Press Space When Done");
            gameUIController.SetPhaseText("Player Turn");
        }


        private void HandleOnAllFishCaught()
        {
            LeanTween.pauseAll();
            currentGamePhase = GamePhase.GAME_WIN;
            SceneManager.LoadScene("GameOverWin");
        }

        private void HandleOnBoatCrash()
        {
            LeanTween.pauseAll();
            currentGamePhase = GamePhase.GAME_OVER;
            SceneManager.LoadScene("GameOverLose");
        }


        private void HandleOnBoatRotateComplete()
        {
            gameUIController.HideHelpText();
            currentGamePhase = GamePhase.MOVE_BOAT;
            boatController.StartMoving();
        }

        private void HandleOnBoatMoveComplete()
        {
            gameUIController.SetPhaseText("Fish Turn");
            currentGamePhase = GamePhase.MOVE_FISH;
            fishManager.MoveFish();
        }

        private void HandleOnAllFishMovementComplete()
        {
            gameUIController.SetPhaseText("Drifting");
            currentGamePhase = GamePhase.DRIFT;
            driftManager.StartDrift();
        }

        private void HandleOnDriftingComplete()
        {
            gameUIController.SetPhaseText("Player Turn");
            currentGamePhase = GamePhase.ROTATE_BOAT;
        }
    }
}