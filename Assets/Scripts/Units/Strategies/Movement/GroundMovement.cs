using Maps.MapManagement.Grid;
using UnityEngine;

namespace Units.Strategies.Movement
{
    [RequireComponent(typeof(Unit))]
    public class GroundMovement : MonoBehaviour, IMovementStrategy
    {
        private Unit _unit;
        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        public void Move(Unit unit, Vector3 destination, float speed)
        {
            var node = GridManager.Instance.GetNodeFromWorldPoint(destination);
            if (node == null)
            {
                Debug.LogWarning($"{name}: cannot find grid node at {destination}");
                return;
            }
            unit.Mover.OnDirectionChanged += OnDirectionChanged;
            unit.Mover.MoveTo(node, speed);
        }


        public void Stop(Unit unit)
        {
            unit.Mover.ForceToStop();
            unit.Mover.OnDirectionChanged -= OnDirectionChanged;
        }
        
        private void OnDirectionChanged(Vector2 dir)
        {
            _unit.Animator.ChangeMovingDirection(dir);
        }
    }
}