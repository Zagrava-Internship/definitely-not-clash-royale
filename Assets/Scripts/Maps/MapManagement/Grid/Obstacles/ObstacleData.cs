using System;
using System.Collections.Generic;
using System.Linq;
using Maps.MapManagement.Grid.Strategies;
using UnityEngine;

namespace Maps.MapManagement.Grid.Obstacles
{
    [Serializable]
    [CreateAssetMenu(fileName = "MapObstacleData", menuName = "Grid/Obstacles/MapObstacleData")]
    public class ObstacleData: ScriptableObject
    {
        public List<RegionStrategy> regions;
    }
}