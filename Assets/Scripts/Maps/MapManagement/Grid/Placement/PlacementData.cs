using System;
using System.Collections.Generic;
using Maps.MapManagement.Grid.Strategies;
using UnityEngine;

namespace Maps.MapManagement.Grid.Placement
{
    /// <summary>
    /// This class holds the placement data for the grid.
    /// Contains regions that CAN be used for placement.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "PlacementData", menuName = "Grid/Placement/PlacementData")]
    public class PlacementData:ScriptableObject
    {
        public List<RegionStrategy> regions;
    }
}