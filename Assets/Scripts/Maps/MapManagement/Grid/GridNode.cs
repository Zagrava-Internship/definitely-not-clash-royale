using UnityEngine;

namespace Maps.MapManagement.Grid
{
    public class GridNode
    {
        // --- Grid coordinates ---
        public int X { get; }
        public int Y { get; }

        // --- World position ---
        public Vector3 WorldPosition { get; }

        // --- Status flags ---
        public bool IsWalkable { get; set; }
        public bool IsPlaceable { get; set; }

        // --- Pathfinding fields ---
        public GridNode Parent { get; set; }
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost => GCost + HCost;

        // --- Constructor ---
        public GridNode(int x, int y, Vector3 worldPos, bool isWalkable = true)
        {
            X = x;
            Y = y;
            WorldPosition = worldPos;
            IsWalkable = isWalkable;
            IsPlaceable = true; // default true
        }
    }
}