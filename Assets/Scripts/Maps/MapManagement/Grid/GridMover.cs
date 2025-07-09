using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maps.MapManagement.Grid
{
    public class GridMover:MonoBehaviour
    {
        public float moveSpeed = 3f;
        private List<GridNode> path;
        private int targetIndex;
        
        // Invoked when the unit finishes moving along the entire path to the target node.
        public event Action OnPathComplete;
        
        public void MoveTo(GridNode targetNode)
        {
            var startGridNode = GridManager.Instance.GetNodeFromWorldPoint(transform.position);
            if (startGridNode == null)
            {
                Debug.LogError("No valid start node found for movement.");
                return;
            }
            var startGridPos = new Vector2Int(startGridNode.X, startGridNode.Y);
            path = GridManager.Instance.FindPath(startGridPos, new Vector2Int(targetNode.X, targetNode.Y));
            if (path is not { Count: > 0 }) return;
            StopAllCoroutines();
            StartCoroutine(FollowPath());
        }
        private IEnumerator FollowPath()
        {
            targetIndex = 0;
            var currentWaypoint = path[targetIndex].WorldPosition;
            while (true)
            {
                if (Vector3.Distance(transform.position, currentWaypoint) < 0.05f)
                {
                    targetIndex++;
                    if (targetIndex >= path.Count)
                    {
                        OnPathComplete?.Invoke();
                        yield break; 
                    }
                    currentWaypoint = path[targetIndex].WorldPosition;
                }
                transform.position = Vector3.MoveTowards(transform.position, 
                    currentWaypoint, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}