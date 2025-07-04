using UnityEngine;

namespace Grid
{
    public class GridNode
    {
        //public Vector2Int gridPosition; 
        public int x, y;            // Grid coordinates
        public Vector3 worldPosition;   
        public bool isWalkable;         
        public GridNode parent;         // For A* pathfinding
        public float gCost, hCost;      // Cost values for A* pathfinding
        public float fCost => gCost + hCost; // Total cost for A* pathfinding
        public GridNode(int x , int y , Vector3 worldPos, bool walkable = true)
        {
            this.x = x;
            this.y = y;
            worldPosition = worldPos;
            isWalkable = walkable;
        }
    }
}