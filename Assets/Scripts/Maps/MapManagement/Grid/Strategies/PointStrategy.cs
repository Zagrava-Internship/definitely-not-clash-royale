using UnityEngine;

namespace Maps.MapManagement.Grid.Strategies
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