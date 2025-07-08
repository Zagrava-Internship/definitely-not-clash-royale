using UnityEngine;

namespace Grid.Obstacles.ObstaclesStrategy
{
    [CreateAssetMenu(fileName = "CircleStrategy", menuName = "Grid/Obstacles/CircleStrategy")]
    public class CircleStrategy: RegionStrategy
    {
        public override bool IsBlocked(Vector2Int pos)
        {
            var dist = (pos - center).sqrMagnitude;
            return dist <= size.x * size.x;
        }
    }
}