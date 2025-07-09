using System.Linq;
using Maps.MapManagement.Grid.Obstacles;
using UnityEngine;

namespace Maps.MapManagement.Validators
{
    public class ObstacleValidator: IGridValidator
    {
        private readonly ObstacleData _obstacleData;
        public ObstacleValidator(ObstacleData obstacleData)
        {
            _obstacleData = obstacleData;
        }
        public bool IsValid(int x, int y)
        {
            // Maybe we should check if the coordinates are within the grid bounds? HZ
            return !_obstacleData.regions.Any(region => region.IsOccupied(new Vector2Int(x,y)));
        }
    }
}