using UnityEngine;

namespace Maps.MapManagement.Grid.Strategies
{
    [CreateAssetMenu(fileName = "VerticalLineStrategy", menuName = "Grid/Strategies/VerticalLineStrategy")]
    public class VerticalLineStrategy: RegionStrategy
    {
        public override bool IsOccupied(Vector2Int pos)
        {
            var halfLength = size.y / 2;
            var startY = center.y - halfLength + (size.y % 2 ^ 1); 
            var endY = center.y + halfLength;
            return pos.x == center.x &&
                   pos.y >= startY &&
                   pos.y <= endY;
        }
    }
}