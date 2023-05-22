using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Fish
{
    public class FishController : MonoBehaviour
    {
        public Action<FishController> OnFishDoneMoving;
        public Action<FishController> OnFishCaught;

        [Header("External Objects")]
        [SerializeField] private FishMovementArrow fishMovementArrow;
        [SerializeField] private GameObject fishSprite;

        [Header("Tween Speeds")]
        [SerializeField] private float fishRotateSpeed = 1.0f;
        [SerializeField] private float fishMovementSpeed = 1.0f;

        [Header("Movement")]
        [SerializeField] private LayerMask movementLayerMask;

        private int fishMovementDistanceMin = 1;
        private int fishMovementDistanceMax = 4;
        private Rigidbody2D rigidbody;
        private CircleCollider2D collider;
        private float lastFishHeading;
        private float newFishHeading;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<CircleCollider2D>();

            lastFishHeading = fishSprite.transform.rotation.eulerAngles.z;
            fishMovementArrow.OnSpinComplete += HandleOnSpinArrowComplete;
            fishMovementArrow.HideArrow();
        }

        public void MoveFish(int maxFishMove)
        {
            fishMovementDistanceMax = maxFishMove;
            fishMovementArrow.SpinArrow(lastFishHeading - 90.0f);
        }

        private void HandleOnSpinArrowComplete(float angle)
        {
            float angleDifference = Mathf.DeltaAngle(lastFishHeading, (angle % 360) + 90);
            float rotationAmount = Mathf.Clamp(angleDifference, -360, 360);
            newFishHeading = lastFishHeading + rotationAmount;

            LeanTween.value(
                    gameObject,
                    PerformRotateFishTween,
                    lastFishHeading,
                    newFishHeading,
                    fishRotateSpeed
                ).setEase(LeanTweenType.easeInOutCubic)
                .setOnComplete(HandleRotateFishTweenComplete);
        }

        private void PerformRotateFishTween(float value)
        {
            fishSprite.transform.rotation = Quaternion.Euler(0, 0, value);
        }

        private void HandleRotateFishTweenComplete()
        {
            lastFishHeading = newFishHeading;

            Vector3 currentPosition = transform.position;
            Vector3 direction = Quaternion.Euler(0, 0, lastFishHeading) * Vector3.right;
            int movementDistance = Random.Range(fishMovementDistanceMin, fishMovementDistanceMax + 1);
            Vector3 newPosition = TestMovement(currentPosition, direction, movementDistance);

            LeanTween.value(
                    gameObject,
                    PerformMoveFishTween,
                    currentPosition,
                    newPosition,
                    fishMovementSpeed
                ).setEase(LeanTweenType.easeInOutCubic)
                .setOnComplete(() => { OnFishDoneMoving?.Invoke(this); });
        }

        private void PerformMoveFishTween(Vector3 tweenedPosition)
        {
            rigidbody.MovePosition(tweenedPosition);
        }

        private Vector3 TestMovement(Vector3 currentPosition, Vector3 direction, float distance)
        {
            RaycastHit2D hit = Physics2D.CircleCast(
                currentPosition,
                collider.radius,
                direction,
                distance,
                movementLayerMask
            );
            if (hit.collider != null)
            {
                return currentPosition + (direction.normalized * (hit.distance - 0.05f));
            }

            return currentPosition + (direction.normalized * distance);
        }

        public void Caught()
        {
            LeanTween.pause(gameObject);
            OnFishCaught?.Invoke(this);
            Destroy(gameObject);
        }
    }
}