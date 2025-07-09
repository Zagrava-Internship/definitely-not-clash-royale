using System.Collections.Generic;
using Maps.MapManagement.Grid;
using UnityEngine;

namespace Maps.MapManagement.Pathfinding.Utils
{
    public static class PathfinderUtils
    {
        public static GridNode GetLowestFCostNode(List<GridNode> openSet)
        {
            var lowestNode = openSet[0];
            for (var i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < lowestNode.fCost || 
                    (Mathf.Approximately(openSet[i].fCost, lowestNode.fCost) && openSet[i].hCost < lowestNode.hCost))
                {
                    lowestNode = openSet[i];
                }
            }
            return lowestNode;
        }

        public static int GetDistance(GridNode a, GridNode b)
        {
            var dstX = Mathf.Abs(a.x - b.x);
            var dstY = Mathf.Abs(a.y - b.y);
            return (dstX > dstY) ? (14 * dstY + 10 * (dstX - dstY)) : (14 * dstX + 10 * (dstY - dstX));
        }

        public static List<GridNode> RetracePath(GridNode startNode, GridNode endNode)
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
    }
}