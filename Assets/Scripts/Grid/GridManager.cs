using UnityEngine;

namespace Grid
{
    public class GridManager:MonoBehaviour
    {
        [Header("Grid Settings")]
        public uint width = 18;
        public uint height = 29;
        [Header("Cell Settings")]
        public float cellWidth = 1f;
        public float cellHeight = 1f;
        [Header("Origin Position")]
        public Vector3 originPosition = Vector3.zero;
        private GridNode[,] _grid;
        [Header("Debug Grid Regeneration")]
        public bool autoRegenerate = true;      
        public float regenInterval = 2f;         
        private float regenTimer = 0f;
        public static GridManager Instance { get; private set; }
        private void Awake() {
            Instance = this;
            GenerateGrid();
        }
        void Update()
        {
            if (!autoRegenerate) return;

            regenTimer += Time.deltaTime;
            if (!(regenTimer >= regenInterval)) return;
            regenTimer = 0f;
            GenerateGrid();
        }
        private void GenerateGrid() {
            _grid = new GridNode[width, height];
            for (var x = 0; x < width; x++) {
                for (var y = 0; y < height; y++) {
                    Vector3 worldPos = originPosition + new Vector3(x * cellWidth, y * cellHeight, 0f);
                    _grid[x, y] = new GridNode(x,y, worldPos);
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

            Gizmos.color = Color.cyan;
            foreach (var node in _grid) {
                Gizmos.DrawWireCube(node.worldPosition, new Vector3(cellWidth, cellHeight, -5f));
            }
        }
    }
}