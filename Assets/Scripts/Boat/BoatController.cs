using System;
using DiceRoller;
using Embugerance;
using Fish;
using Game;
using UnityEngine;

namespace Boat
{
    public class BoatController : MonoBehaviour
    {
        public Action OnBoatRotateComplete;
        public Action OnBoatCrash;
        public Action OnBoatMoveComplete;
        public Action OnBoatDriftComplete;

        [SerializeField] private float rotateSpeed = 1.0f;
        [SerializeField] private int movementDistance = 6;
        [SerializeField] private float movementTweenSpeed = 1.0f;
        [SerializeField] private LayerMask movementLayerMask;
        [SerializeField] private DiceRollController diceRollController;

        private Rigidbody2D rigidbody;
        private PolygonCollider2D collider;
        private float driftHeading;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<PolygonCollider2D>();
            diceRollController.OnDiceRollCompleted += HandleDiceRolled;
        }


        private void Update()
        {
            if (GameManager.S.IsGamePaused()) return;

            HandleBoatRotationInput();

            if (GameManager.S.GetCurrentPhase() == GamePhase.ROTATE_BOAT)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    OnBoatRotateComplete?.Invoke();
                }
            }
        }

        private void HandleBoatRotationInput()
        {
            int direction = 0;
            if (Input.GetKey(KeyCode.A))
            {
                direction = 1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                direction = -1;
            }

            if (direction != 0)
            {
                transform.Rotate(0, 0, direction * rotateSpeed * Time.deltaTime);
            }
        }

        public void HandleDrifting(float heading, int maxDrift)
        {
            driftHeading = heading;
            diceRollController.RollDice(1, maxDrift);
        }

        private void HandleDiceRolled(int value)
        {
            if (GameManager.S.GetCurrentPhase() == GamePhase.DRIFT)
            {
                InternalHandleDrift(value);
            } else if (GameManager.S.GetCurrentPhase() == GamePhase.MOVE_BOAT)
            {
                InternalHandleMovement(value);
            }
        }

        private void InternalHandleMovement(int value)
        {
            Vector3 currentPosition = rigidbody.position;
            Vector3 direction = Quaternion.Euler(0, 0, rigidbody.rotation) * Vector3.up;

            Vector3 rbPos = rigidbody.position;
            Vector3 newPosition = rbPos + (direction.normalized * value);


            LeanTween.value(
                    gameObject,
                    PerformBoatMovementTween,
                    currentPosition,
                    newPosition,
                    movementTweenSpeed
                ).setEase(LeanTweenType.easeInOutCubic)
                .setOnComplete(() => OnBoatMoveComplete?.Invoke());
        }

        private void InternalHandleDrift(int value)
        {
            int driftAmount = value;
            Vector3 currentPosition = rigidbody.position;
            Vector3 direction = Quaternion.Euler(0, 0, driftHeading) * Vector3.up;

            Vector3 rbPos = rigidbody.position;
            Vector3 newPosition = rbPos + (direction.normalized * driftAmount);


            LeanTween.value(
                    gameObject,
                    PerformBoatMovementTween,
                    currentPosition,
                    newPosition,
                    movementTweenSpeed
                ).setEase(LeanTweenType.easeInOutCubic)
                .setOnComplete(() => OnBoatDriftComplete?.Invoke());
        }

        public void StartMoving()
        {
            if (EmbuggeranceManager.S.HasEmbuggerance(EmbuggeranceType.Binding))
            {
                diceRollController.RollDice(1, 6);
            }
            else
            {
                InternalHandleMovement(movementDistance);
            }
        }

        private void PerformBoatMovementTween(Vector3 pos)
        {
            rigidbody.MovePosition(pos);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                LeanTween.pause(gameObject);
                OnBoatCrash?.Invoke();
            }
            else if (other.gameObject.TryGetComponent(out FishController fishController))
            {
                fishController.Caught();
            }
            else if (other.gameObject.TryGetComponent(out Obstacle obstacle))
            {
                if (obstacle.GetObstacleType() == ObstacleType.Rock)
                {
                    LeanTween.pause(gameObject);
                    OnBoatCrash?.Invoke();
                }
            }
        }
    }
}