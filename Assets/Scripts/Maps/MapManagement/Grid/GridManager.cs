using System.Collections.Generic;
using Maps.MapManagement.Grid.Obstacles;
using Maps.MapManagement.Pathfinding;
using UnityEngine;

namespace Maps.MapManagement.Grid
{
    public class GridManager:MonoBehaviour
    {
        [Header("Grid Settings Data")]
        public MapGridSettingsData gridSettingsData;
        [Header("Obstacle Settings")]
        public MapObstacleData obstacleData;
        
        private GridData GridData { get; set; }
        private Pathfinder Pathfinder { get; set; }
    
        public static GridManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            InitializeSystems();
        }
        private void InitializeSystems()
        {
            if (gridSettingsData == null)
            {
                Debug.LogError("GridSettingsData is not assigned in the GridManager!");
                return;
            }
            GridData = new GridData(gridSettingsData,obstacleData);
            Pathfinder = new Pathfinder(GridData);
        }
        /// <summary>
        /// Finds a path between two points in the world space.
        /// </summary>
        public List<GridNode> FindPath(Vector3 startWorldPos, Vector3 targetWorldPos)
        {
            var startNode = GridData.GetClosestNode(startWorldPos);
            var targetNode = GridData.GetClosestNode(targetWorldPos);

            if (startNode == null || targetNode == null) return null;

            return Pathfinder.FindPath(
                new Vector2Int(startNode.x, startNode.y), 
                new Vector2Int(targetNode.x, targetNode.y)
            );
        }
        /// <summary>
        /// Finds a path between two grid positions.
        /// </summary>
        public List<GridNode> FindPath(Vector2Int startGridPos, Vector2Int targetGridPos)
        {
            return Pathfinder.FindPath(startGridPos, targetGridPos);
        }
        /// <summary>
        ///  Gets the closest grid node to a given world position.
        /// </summary>
        public GridNode GetNodeFromWorldPoint(Vector3 worldPosition)
        {
            return GridData.GetClosestNode(worldPosition);
        }
    }
}