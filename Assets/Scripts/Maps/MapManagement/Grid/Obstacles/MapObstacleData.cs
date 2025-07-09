using System;
using System.Collections.Generic;
using System.Linq;
using Maps.MapManagement.Grid.Obstacles.ObstaclesStrategy;
using UnityEngine;

namespace Maps.MapManagement.Grid.Obstacles
{
    [Serializable]
    [CreateAssetMenu(fileName = "MapObstacleData", menuName = "Grid/Obstacles/MapObstacleData")]
    public class MapObstacleData: ScriptableObject
    {
        public List<RegionStrategy> regions;
        public bool IsBlocked(Vector2Int pos)
        {
            return regions.Any(region => region.IsBlocked(pos));
        }
    }
}