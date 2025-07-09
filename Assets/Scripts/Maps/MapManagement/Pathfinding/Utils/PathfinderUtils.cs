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
                if (openSet[i].FCost < lowestNode.FCost || 
                    (Mathf.Approximately(openSet[i].FCost, lowestNode.FCost) && openSet[i].HCost < lowestNode.HCost))
                {
                    lowestNode = openSet[i];
                }
            }
            return lowestNode;
        }

        public static int GetDistance(GridNode a, GridNode b)
        {
            var dstX = Mathf.Abs(a.X - b.X);
            var dstY = Mathf.Abs(a.Y - b.Y);
            return (dstX > dstY) ? (14 * dstY + 10 * (dstX - dstY)) : (14 * dstX + 10 * (dstY - dstX));
        }

        public static List<GridNode> RetracePath(GridNode startNode, GridNode endNode)
        {
            var path = new List<GridNode>();
            var currentNode = endNode;
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Reverse();
            return path;
        }
    }
}