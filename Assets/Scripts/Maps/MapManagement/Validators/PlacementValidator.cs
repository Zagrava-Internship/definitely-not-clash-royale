using System.Linq;
using Maps.MapManagement.Grid.Placement;
using UnityEngine;

namespace Maps.MapManagement.Validators
{
    public class PlacementValidator:IGridValidator
    {
        private readonly PlacementData _placementData;
        public PlacementValidator(PlacementData placementData)
        {
            _placementData = placementData;
        }
        public bool IsValid(int x, int y)
        {
            return _placementData.regions.Any(el => el.IsOccupied(new Vector2Int(x,y)));
        }
    }
}