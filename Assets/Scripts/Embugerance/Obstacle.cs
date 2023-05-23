using UnityEngine;

namespace Embugerance
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private ObstacleType obstacleType;

        public ObstacleType GetObstacleType() => obstacleType;
    }
}