using UnityEngine;

namespace Maps.MapManagement.Grid.Strategies
{
    [CreateAssetMenu(fileName = "RectangleStrategy", menuName = "Grid/Strategies/RectangleStrategy")]
    public class RectangleStrategy: RegionStrategy
    {
        public override bool IsOccupied(Vector2Int pos)
        {
            var minX = center.x - size.x / 2;
            var maxX = center.x + (size.x - 1) / 2;

            var minY = center.y - size.y / 2;
            var maxY = center.y + (size.y - 1) / 2;

            return pos.x >= minX && pos.x <= maxX && 
                   pos.y >= minY && pos.y <= maxY;
        }
    }
}