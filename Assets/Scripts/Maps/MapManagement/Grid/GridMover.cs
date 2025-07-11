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
        
        // The last direction the unit moved in, used to determine if the direction has changed.
        private Vector2 _lastDirection;
        
        // Invoked when the unit finishes moving along the entire path to the target node.
        public event Action OnPathComplete;
        public event Action<Vector2> OnDirectionChanged;
        
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
            UpdateDirection(currentWaypoint);

            while (true)
            {
                var targetPos = new Vector3(currentWaypoint.x, currentWaypoint.y, transform.position.z);

                if (Vector3.Distance(transform.position, targetPos) < 0.05f)
                {
                    _targetIndex++;
                    if (_targetIndex >= _path.Count)
                    {
                        OnPathComplete?.Invoke();
                        yield break; 
                    }
                    currentWaypoint = _path[_targetIndex].WorldPosition;
                    UpdateDirection(currentWaypoint);
                    targetPos = new Vector3(currentWaypoint.x, currentWaypoint.y, transform.position.z);
                }

                transform.position = Vector3.MoveTowards(transform.position, 
                    targetPos, _moveSpeed * Time.deltaTime);
            
                yield return null;
            }
        }
        private void UpdateDirection(Vector2 targetPosition)
        {
            // Calculate the current position in 2D space
            var currentPosition = new Vector2(transform.position.x, transform.position.y);
            // Calculate the displacement vector from the current position to the target position
            var displacement = targetPosition - currentPosition;
            
            const float threshold = 0.05f; // This is used to determine if the displacement is negligible.
            // The bigger the threshold, the more "quantized" the direction will be.
            
            // Quantize the direction based on the displacement vector.
            var x = Mathf.Abs(displacement.x) < threshold ? 0 : Mathf.Sign(displacement.x);
            var y = Mathf.Abs(displacement.y) < threshold ? 0 : Mathf.Sign(displacement.y);

            var quantizedDirection = new Vector2(x, y);

            if (quantizedDirection == _lastDirection) return;

            _lastDirection = quantizedDirection;
            OnDirectionChanged?.Invoke(_lastDirection);
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