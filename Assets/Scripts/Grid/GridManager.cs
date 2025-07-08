using System.Collections.Generic;
using Grid.Obstacles;
using Maps;
using UnityEngine;

namespace Grid
{
    public class GridManager:MonoBehaviour
    {
        [Header("Preload Settings")]
        public bool usePreloadedSettings = false;
        [Header("Preloaded Grid Settings Data")]
        public MapGridSettingsData gridSettingsData;
        [Header("Grid Settings")]
        public uint width = 18;
        public uint height = 29;
        [Header("Cell Settings")]
        public float cellWidth = 1f;
        public float cellHeight = 1f;
        [Header("Origin Position")]
        public Vector3 originPosition = Vector3.zero;
        private GridNode[,] _grid;
        [Header("Grid obstacles")]
        public MapObstacleData obstacleData;
        [Header("Debug Grid Regeneration")]
        public bool autoRegenerate = true;      
        public float regenInterval = 2f;         
        private float regenTimer = 0f;
        public static GridManager Instance { get; private set; }
        private void Awake() {
            Instance = this;
            GenerateGrid();
        }
        private void Update()
        {
            if (!autoRegenerate) return;

            regenTimer += Time.deltaTime;
            if (!(regenTimer >= regenInterval)) return;
            regenTimer = 0f;
            GenerateGrid();
        }
        private void ApplySettings()
        {
            if (!usePreloadedSettings || gridSettingsData == null) return;
            width = gridSettingsData.width;
            height = gridSettingsData.height;
            cellWidth = gridSettingsData.cellWidth;
            cellHeight = gridSettingsData.cellHeight;
            originPosition = gridSettingsData.originPosition;
            autoRegenerate = gridSettingsData.autoRegenerate;
            regenInterval = gridSettingsData.regenInterval;
        }
        private void GenerateGrid() {
            _grid = new GridNode[width, height];
            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    var worldPos = originPosition + new Vector3(x * cellWidth, y * cellHeight, 0f);
                    _grid[x, y] = new GridNode(x,y, worldPos)
                    {
                        isWalkable = true // Default to walkable
                    };
                    var isBlocked = obstacleData?.IsBlocked(new Vector2Int(x, y));
                    if(isBlocked.HasValue && isBlocked.Value) {
                        _grid[x, y].isWalkable = false; 
                    }
                }
            }
        }
        public GridNode GetNode(Vector2Int gridPos)=>
            IsInBounds(gridPos) ? _grid[gridPos.x, gridPos.y] : null;
        public bool IsInBounds(Vector2Int pos) =>
            pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
        public GridNode GetClosestNode(Vector3 worldPos) {
            var x = Mathf.RoundToInt((worldPos.x - originPosition.x) / cellWidth);
            var y = Mathf.RoundToInt((worldPos.y - originPosition.y) / cellHeight);
            return GetNode(new Vector2Int(x, y));
        }
        /// <summary>
        /// This method is used to visualize the grid in the editor.
        /// Only used for debugging and visualization, not for runtime logic.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (_grid == null) return;

            foreach (var node in _grid)
            {
                Gizmos.color= node.isWalkable? Color.cyan : new Color(1f, 0f, 0f, 0.4f);
                if (node.isWalkable)
                    Gizmos.DrawWireCube(node.worldPosition, new Vector3(cellWidth, cellHeight, 0.1f));
                else
                    Gizmos.DrawCube(node.worldPosition, new Vector3(cellWidth, cellHeight, 0.1f));
            }
        }
        public List<GridNode> FindPath(Vector2Int startPos, Vector2Int targetPos)
        {
            if (!IsInBounds(startPos) || !IsInBounds(targetPos))
            {
                Debug.LogWarning("Start or target position is out of bounds!");
                return null;
            }
            var startNode = GetNode(startPos);
            var targetNode = GetNode(targetPos);
            var openSet = new List<GridNode>();
            var closedSet = new HashSet<GridNode>();

            openSet.Add(startNode);
            
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var node = _grid[x, y];
                    node.gCost = int.MaxValue;
                    node.hCost = 0;
                    node.parent = null;
                }
            }
            startNode.gCost = 0;
            startNode.hCost = GetDistance(startNode, targetNode);

            while (openSet.Count > 0)
            {
                var currentNode = openSet[0];
                for (var i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < currentNode.fCost || 
                        (Mathf.Approximately(openSet[i].fCost, currentNode.fCost) && 
                         openSet[i].hCost < currentNode.hCost))
                    {
                        currentNode = openSet[i];
                    }
                }
                if (currentNode == targetNode)
                {
                    return RetracePath(startNode, targetNode);
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                foreach (var neighbor in GetNeighbors(currentNode))
                {
                    if (!neighbor.isWalkable || closedSet.Contains(neighbor))
                    {
                        continue;
                    }
                    var tentativeGCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                    if (!(tentativeGCost < neighbor.gCost)) continue;
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
            return null;
        }
        private static List<GridNode> RetracePath(GridNode startNode, GridNode endNode)
        {
            var path = new List<GridNode>();
            var currentNode = endNode;
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }
            path.Reverse();
            return path;
        }
        private static int GetDistance(GridNode a, GridNode b)
        {
            var dstX = Mathf.Abs(a.x - b.x);
            var dstY = Mathf.Abs(a.y - b.y);
            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
        private List<GridNode> GetNeighbors(GridNode node)
        {
            var neighbors = new List<GridNode>();
            var directions = new int[,]
            {
                {  0,  1 },  // up
                {  1,  0 },  // right
                {  0, -1 },  // down
                { -1,  0 },  // left
                {  1,  1 },  // right-up (↗)
                {  1, -1 },  // right-down (↘)
                { -1, -1 },  // left-down (↙)
                { -1,  1 }   // left-up (↖)
            };
            for (var i = 0; i < directions.GetLength(0); i++)
            {
                var checkX = node.x + directions[i, 0];
                var checkY = node.y + directions[i, 1];
                if (IsInBounds(new Vector2Int(checkX, checkY)))
                {
                    neighbors.Add(_grid[checkX, checkY]);
                }
            }
            return neighbors;
        }
    }
}