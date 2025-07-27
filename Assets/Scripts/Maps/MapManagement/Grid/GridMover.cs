using System;
using System.Collections;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Maps.MapManagement.Grid
{
    public class GridMover:MonoBehaviour
    {
        private const float Threshold = 0.1f; // Threshold to avoid jittering when determining a direction
        
        private float _moveSpeed;
        private List<GridNode> _path;
        private int _targetIndex;
        
        // The last direction the unit moved in, used to determine if the direction has changed.
        private Vector2 _lastDirection;
        
        public event Action<Vector2> OnDirectionChanged;
        public event Action OnMovementUpdate; // Optional event to notify when movement updates occur
        
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


            StopAllCoroutines();
            StartCoroutine(FollowPath());
        }
        private IEnumerator FollowPath()
        {
            if (_path == null || _path.Count == 0)
            {
                yield break;
            }

            _targetIndex = 0;
            
            var currentWaypoint = _path[_targetIndex].WorldPosition; 
            UpdateDirection(currentWaypoint);

            while (true)
            {
                var targetPos = new Vector3(currentWaypoint.x, currentWaypoint.y, transform.position.z);

                if (Vector3.Distance(transform.position, targetPos) < Threshold)
                {
                    _targetIndex++;
                    if (_targetIndex >= _path.Count)
                    {
                        yield break; 
                    }
                    currentWaypoint = _path[_targetIndex].WorldPosition;
                    var movementVector = (Vector2)currentWaypoint - (Vector2)transform.position;
                    UpdateDirection(movementVector);
                    targetPos = new Vector3(currentWaypoint.x, currentWaypoint.y, targetPos.y * -0.01f);// Ensure z position is consistent
                }

                transform.position = Vector3.MoveTowards(transform.position, 
                    targetPos, _moveSpeed * Time.deltaTime);
                OnMovementUpdate?.Invoke(); // Notify that movement has been updated
            
                yield return null;
            }
        }
        private void UpdateDirection(Vector2 movementVector)
        {
            var x = Mathf.Abs(movementVector.x) < Threshold ? 0 : Mathf.Sign(movementVector.x);
            var y = Mathf.Abs(movementVector.y) < Threshold ? 0 : Mathf.Sign(movementVector.y);

            var quantizedDirection = new Vector2(x, y);

            if (quantizedDirection == Vector2.zero || quantizedDirection == _lastDirection) return;

            _lastDirection = quantizedDirection;
            OnDirectionChanged?.Invoke(_lastDirection);
        }
        public void ForceToStop()
        {
             StopAllCoroutines();
            _path = null;
            _targetIndex = 0; 
        }
    }
}