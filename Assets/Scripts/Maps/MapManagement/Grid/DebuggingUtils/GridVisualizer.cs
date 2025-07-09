using UnityEngine;

namespace Maps.MapManagement.Grid.DebuggingUtils
{
    public static class GridVisualizer
    {
        public static void Draw(GridData gridData)
        {
            for (var x = 0; x < gridData.Grid.GetLength(0); x++)
            {
                for (var y = 0; y < gridData.Grid.GetLength(1); y++)
                {
                    var node = gridData.Grid[x, y];
                    // Calculate position relative to world position 
                    var pos = new Vector3(
                        x * gridData.CellWidth + gridData.OriginPosition.x,
                        y * gridData.CellHeight + gridData.OriginPosition.y,
                        0 // Assuming a 2D grid, Z can be 0 or adjusted for depth
                    );
                    // Prioritize IsPlaceable for visualization
                    if (node.IsPlaceable)
                    {
                        Gizmos.color = Color.green;
                    }
                    else if (node.IsWalkable)
                    {
                        Gizmos.color = Color.blue; // Walkable
                    }
                    else
                    {
                        Gizmos.color = Color.red; // Non-walkable
                    }
                    Gizmos.DrawCube(pos, new Vector3(gridData.CellWidth, gridData.CellHeight, 1));
                }
            }
        }
    }
}