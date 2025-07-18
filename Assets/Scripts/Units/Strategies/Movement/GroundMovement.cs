using Maps.MapManagement.Grid;
using Units.StateMachine;
using UnityEngine;

namespace Units.Strategies.Movement
{
    [RequireComponent(typeof(Unit))]
    public class GroundMovement : MonoBehaviour, IMovementStrategy
    {
        private Unit _unit;
        private GridMover _mover;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
            _mover = _unit.Mover; 
        }


        public void Move(Unit unit, Vector3 destination, float speed)
        {
            var node = GridManager.Instance.GetNodeFromWorldPoint(destination);
            if (node == null)
            {
                Debug.LogWarning($"{name}: cannot find grid node at {destination}");
                return;
            }
            _mover.OnPathComplete += OnPathComplete;
            _mover.OnDirectionChanged += OnDirectionChanged;
            _mover.MoveTo(node, speed);
        }


        public void Stop(Unit unit)
        {
            _mover.ForceToStop();
            _mover.OnPathComplete -= OnPathComplete;
            _mover.OnDirectionChanged -= OnDirectionChanged;
        }

        private void OnPathComplete()
        {
            _mover.OnPathComplete -= OnPathComplete;
            _unit.SetState(new AttackState(_unit, _unit.CurrentTarget));
        }

        private void OnDirectionChanged(Vector2 dir)
        {
            _unit.Animator.ChangeMovingDirection(dir);
        }
    }
}