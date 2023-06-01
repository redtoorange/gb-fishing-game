using UnityEngine;

namespace Player
{
    public class TransformFollower : MonoBehaviour
    {
        [SerializeField] private Transform followTarget;
        
        private Vector3 initialOffset;

        private void Start()
        {
            initialOffset =transform.position -  followTarget.position;
        }

        private void Update()
        {
            transform.position = followTarget.position + initialOffset;
        }
    }
}