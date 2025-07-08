using UnityEngine;

namespace Grid.Obstacles.ObstaclesStrategy
{
    [CreateAssetMenu(fileName = "PointStrategy", menuName = "Grid/Obstacles/PointStrategy")]
    public class PointStrategy : RegionStrategy
    {
        public override bool IsBlocked(Vector2Int pos)
        {
            return pos == center;
        }
    }
}