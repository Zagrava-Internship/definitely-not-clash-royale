using Maps.MapManagement.Grid.Obstacles;
using UnityEngine;

namespace Maps.MapManagement.Grid
{
    public class GridData
    {
        public GridNode[,] Grid { get; private set; }
        private uint Width { get; }
        private uint Height { get; }
        private float CellWidth { get; }
        private float CellHeight { get; }
        private Vector3 OriginPosition { get; }
        private MapObstacleData ObstacleData { get; }
        public GridData(MapGridSettingsData settings,MapObstacleData obstacleData)
        {
            Width = settings.width;
            Height = settings.height;
            CellWidth = settings.cellWidth;
            CellHeight = settings.cellHeight;
            OriginPosition = settings.originPosition;
            ObstacleData = obstacleData;
            GenerateGrid();
        }
        private void GenerateGrid() {
            Grid = new GridNode[Width, Height];
            for (var x = 0; x < Width; x++) {
                for (var y = 0; y < Height; y++) {
                    var worldPos = OriginPosition + new Vector3(x * CellWidth, y * CellHeight, 0f);
                    Grid[x, y] = new GridNode(x,y, worldPos)
                    {
                        isWalkable = true // Default to walkable
                    };
                    var isBlocked = ObstacleData?.IsBlocked(new Vector2Int(x, y));
                    if(isBlocked.HasValue && isBlocked.Value) {
                        Grid[x, y].isWalkable = false; 
                    }
                }
            }
        }
        public GridNode GetNode(Vector2Int gridPos)=>
            IsInBounds(gridPos) ? Grid[gridPos.x, gridPos.y] : null;
        private bool IsInBounds(Vector2Int pos) =>
            pos.x >= 0 && pos.x < Width && pos.y >= 0 && pos.y < Height;
        public GridNode GetClosestNode(Vector3 worldPos) {
            var x = Mathf.RoundToInt((worldPos.x - OriginPosition.x) / CellWidth);
            var y = Mathf.RoundToInt((worldPos.y - OriginPosition.y) / CellHeight);
            return GetNode(new Vector2Int(x, y));
        }
    }
}