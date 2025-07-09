using UnityEngine;

namespace Maps.MapManagement.Grid.Strategies
{
    [CreateAssetMenu(fileName = "PointStrategy", menuName = "Grid/Strategies/PointStrategy")]
    public class PointStrategy : RegionStrategy
    {
        public override bool IsOccupied(Vector2Int pos)
        {
            return pos == center;
        }
    }
}