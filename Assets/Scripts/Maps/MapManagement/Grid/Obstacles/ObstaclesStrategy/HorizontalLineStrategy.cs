using UnityEngine;

namespace Maps.MapManagement.Grid.Obstacles.ObstaclesStrategy
{
    [CreateAssetMenu(fileName = "HorizontalLineStrategy", menuName = "Grid/Obstacles/HorizontalLineStrategy")]
    public class HorizontalLineStrategy: RegionStrategy
    {
        public override bool IsBlocked(Vector2Int pos)
        {
            var halfLength = size.x / 2;
            var startX=center.x - halfLength + (size.x % 2 ^ 1);
            var endX=center.x + halfLength;
            return pos.y == center.y && pos.x >= startX && pos.x <= endX;
        }
    }
}