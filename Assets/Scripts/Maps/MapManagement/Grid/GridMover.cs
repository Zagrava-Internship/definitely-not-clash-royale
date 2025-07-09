using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Maps.MapManagement.Grid
{
    public class GridMover:MonoBehaviour
    {
        private float _moveSpeed;
        private List<GridNode> _path;
        private int _targetIndex;
        
        // Invoked when the unit finishes moving along the entire path to the target node.
        public event Action OnPathComplete;
        
        public void MoveTo(GridNode targetNode,float speed)
        {
            _moveSpeed = speed;
            var startGridNode = GridManager.Instance.GetNodeFromWorldPoint(transform.position);
            if (startGridNode == null)
            {
                Debug.LogError("No valid start node found for movement.");
                return;
            }
            var startGridPos = new Vector2Int(startGridNode.X, startGridNode.Y);
            _path = GridManager.Instance.FindPath(startGridPos, new Vector2Int(targetNode.X, targetNode.Y));
            if (_path is not { Count: > 0 }) return;
            StopAllCoroutines();
            StartCoroutine(FollowPath());
        }
        private IEnumerator FollowPath()
        {
            _targetIndex = 0;
            var currentWaypoint = _path[_targetIndex].WorldPosition;
            while (true)
            {
                if (Vector3.Distance(transform.position, currentWaypoint) < 0.05f)
                {
                    _targetIndex++;
                    if (_targetIndex >= _path.Count)
                    {
                        // Reached the end of the path, invoke the completion event
                        StopAllCoroutines();
                        OnPathComplete?.Invoke();
                        yield break; 
                    }
                    currentWaypoint = _path[_targetIndex].WorldPosition;
                }
                transform.position = Vector3.MoveTowards(transform.position, 
                    currentWaypoint, _moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
        public void ForceToStop()
        {
            StopAllCoroutines();
            _path = null;
            _targetIndex = 0;
            OnPathComplete?.Invoke(); // Notify that the path is complete even if stopped
        }
    }
}