using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Embugerance
{
    public class RockManager : MonoBehaviour
    {
        [SerializeField] private Transform tableCenter;
        [SerializeField] private List<Obstacle> rockPrefabs;
        [SerializeField] private int rockCount = 3;
        [SerializeField] private int rockMinScatterDistance = 1;
        [SerializeField] private int rockMaxScatterDistance = 8;

        public void HandleSetup()
        {
            for (int i = 0; i < rockCount; i++)
            {
                Obstacle instance = Instantiate(
                    rockPrefabs[Random.Range(0, rockPrefabs.Count())],
                    GetScatterPosition(),
                    Quaternion.identity,
                    transform
                );
            }
        }

        private Vector3 GetScatterPosition()
        {
            Vector3 position = tableCenter.position;
            int distance = Random.Range(rockMinScatterDistance, rockMaxScatterDistance + 1);
            float angle = Random.Range(0.0f, 360.0f);
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;
            return position + (direction.normalized * distance);
        }
    }
}