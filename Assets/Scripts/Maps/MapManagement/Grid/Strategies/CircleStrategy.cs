using UnityEngine;

namespace Maps.MapManagement.Grid.Strategies
{
    [CreateAssetMenu(fileName = "CircleStrategy", menuName = "Grid/Strategies/CircleStrategy")]
    public class CircleStrategy: RegionStrategy
    {
        public override bool IsOccupied(Vector2Int pos)
        {
            var dist = (pos - center).sqrMagnitude;
            return dist <= size.x * size.x;
        }
    }
}