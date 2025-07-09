using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Maps.MapManagement.Grid.DebuggingUtils;
using Maps.MapManagement.Grid.Obstacles;
using Maps.MapManagement.Grid.Placement;
using Maps.MapManagement.Pathfinding;
using Maps.MapManagement.Validators;
using UnityEngine;

namespace Maps.MapManagement.Grid
{
    public class GridManager:MonoBehaviour
    {
        [Header("Grid Settings Data")]
        public GridSettingsData gridSettingsData;
        [Header("Obstacle Settings")]
        public ObstacleData obstacleData;
        [Header("Placement Settings")]
        public PlacementData placementSettings;
        
        private GridData GridData { get; set; }
        private Pathfinder Pathfinder { get; set; }
        private ValidationManager ValidationManager { get; set; }
        
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

            if (placementSettings == null)
            {
                Debug.LogError("PlacementSettings is not assigned in the GridManager!");
                return;
            }
            // Register the subsystems in the correct order
            ValidationManager = new ValidationManager(obstacleData, placementSettings);//1
            GridData = new GridData(gridSettingsData,obstacleData,ValidationManager);//2
            Pathfinder = new Pathfinder(GridData);//3
            // No dependency injection is a pain :(
        }
        private void OnDrawGizmos()
        {
            if (GridData == null)
                return;
            GridVisualizer.Draw(GridData);
        }

        #region Public API
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
        /// <summary>
        /// Gets the closest grid node with checking if it`s placeable.
        /// Used fot placement validation in CardDropHandler.
        /// </summary>
        [CanBeNull]
        public GridNode GetPlaceableNodeFromWorldPoint(Vector3 worldPosition)
        {
            var node = GridData.GetClosestNode(worldPosition);
            if (node != null && ValidationManager.IsPlaceable(node.X, node.Y))
                return node;
            return null;
        }
        #endregion
    }
}