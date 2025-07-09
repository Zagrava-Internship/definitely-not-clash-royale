using System.Collections.Generic;
using Maps.MapManagement.Grid;
using Maps.MapManagement.Pathfinding.Utils;
using UnityEngine;

namespace Maps.MapManagement.Pathfinding
{
    public class Pathfinder
    {
        private readonly GridData _gridData;

        public Pathfinder(GridData gridData)
        {
            _gridData = gridData;
        }
        public List<GridNode> FindPath(Vector2Int startPos, Vector2Int targetPos)
        {
            var startNode = _gridData.GetNode(startPos);
            var targetNode = _gridData.GetNode(targetPos);

            if (startNode == null || targetNode is not { isWalkable: true })
            {
                Debug.LogWarning("Start/Target node is invalid or target is not walkable.");
                return null;
            }
            var openSet = new List<GridNode>();
            var closedSet = new HashSet<GridNode>();
            openSet.Add(startNode);
            
            foreach(var node in _gridData.Grid)
            {
                node.gCost = int.MaxValue;
                node.parent = null;
            }

            startNode.gCost = 0;
            startNode.hCost = PathfinderUtils.GetDistance(startNode, targetNode);

            while (openSet.Count > 0)
            {
                var currentNode = PathfinderUtils.GetLowestFCostNode(openSet);

                if (currentNode == targetNode)
                {
                    return PathfinderUtils.RetracePath(startNode, targetNode);
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                foreach (var neighbor in GetNeighbors(currentNode))
                {
                    if (!neighbor.isWalkable || closedSet.Contains(neighbor)) continue;

                    var tentativeGCost = currentNode.gCost + PathfinderUtils.GetDistance(currentNode, neighbor);
                    if (tentativeGCost >= neighbor.gCost) continue;
                    neighbor.parent = currentNode;
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = PathfinderUtils.GetDistance(neighbor, targetNode);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
            return null; 
        }
        private List<GridNode> GetNeighbors(GridNode node)
        {
            var neighbors = new List<GridNode>();
            for (var x = -1; x <= 1; x++)
                for (var y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    var checkPos = new Vector2Int(node.x + x, node.y + y);
                    var neighborNode = _gridData.GetNode(checkPos);
                    if(neighborNode != null)
                        neighbors.Add(neighborNode);
                }
            return neighbors;
        }
    }
}