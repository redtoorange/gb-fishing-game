using System;
using Embugerance;
using Fish;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Boat
{
    public class BoatController : MonoBehaviour
    {
        public Action OnBoatRotateComplete;
        public Action OnBoatCrash;
        public Action OnBoatMoveComplete;
        public Action OnBoatDriftComplete;

        [SerializeField] private float rotateSpeed = 1.0f;
        [SerializeField] private float movementDistance = 6.0f;
        [SerializeField] private float movementTweenSpeed = 1.0f;
        [SerializeField] private LayerMask movementLayerMask;

        private Rigidbody2D rigidbody;
        private PolygonCollider2D collider;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<PolygonCollider2D>();
        }

        private void Update()
        {
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

        public void HandleDrifting(float heading, int amount)
        {
            Vector3 currentPosition = rigidbody.position;
            Vector3 direction = Quaternion.Euler(0, 0, heading) * Vector3.up;

            Vector3 rbPos = rigidbody.position;
            Vector3 newPosition = rbPos + (direction.normalized * amount);


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
            Vector3 currentPosition = rigidbody.position;
            Vector3 direction = Quaternion.Euler(0, 0, rigidbody.rotation) * Vector3.up;

            Vector3 rbPos = rigidbody.position;
            Vector3 newPosition = rbPos + (direction.normalized * GetMovementDistance());


            LeanTween.value(
                    gameObject,
                    PerformBoatMovementTween,
                    currentPosition,
                    newPosition,
                    movementTweenSpeed
                ).setEase(LeanTweenType.easeInOutCubic)
                .setOnComplete(() => OnBoatMoveComplete?.Invoke());
        }

        private float GetMovementDistance()
        {
            if (EmbuggeranceManager.S.HasEmbuggerance(EmbuggeranceType.Binding))
            {
                return Random.Range(0, 6) + 1;
            }

            return movementDistance;
        }

        private void PerformBoatMovementTween(Vector3 pos)
        {
            rigidbody.MovePosition(pos);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("Collision occured!");
            if (other.gameObject.CompareTag("Wall"))
            {
                Debug.Log("Hit a Wall!");
                LeanTween.pause(gameObject);
                OnBoatCrash?.Invoke();
            }
            else if (other.gameObject.TryGetComponent(out FishController fishController))
            {
                Debug.Log("Caught a Fish!");
                fishController.Caught();
            }
        }
    }
}