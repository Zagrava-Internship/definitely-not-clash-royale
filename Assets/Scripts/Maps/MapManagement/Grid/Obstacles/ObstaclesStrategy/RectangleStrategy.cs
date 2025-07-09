using UnityEngine;

namespace Maps.MapManagement.Grid.Obstacles.ObstaclesStrategy
{
    public class RectangleStrategy: RegionStrategy
    {
        public override bool IsBlocked(Vector2Int pos)
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