using Maps.MapManagement.Grid.Obstacles;
using Maps.MapManagement.Grid.Placement;

namespace Maps.MapManagement.Validators
{
    public class ValidationManager
    {
        private readonly IGridValidator _obstacleValidator;
        private readonly IGridValidator _placementValidator;

        public ValidationManager(ObstacleData obstacleData, PlacementData placementData)
        {
            _obstacleValidator = new ObstacleValidator(obstacleData);
            _placementValidator = new PlacementValidator(placementData);
        }
        public bool IsObstacle(int x, int y) => !_obstacleValidator.IsValid(x, y);
        public bool IsPlaceable(int x, int y) => !IsObstacle(x, y) && _placementValidator.IsValid(x, y);
    }
}