using Maps.MapManagement.Grid;
using Units.Animation;
using UnityEngine;

namespace Units.UnitStates
{
    public class MoveState : UnitState
    {
        private readonly GridNode _targetNode;

        public MoveState(Unit unit, GridNode node) : base(unit)
        {
            _targetNode = node;
        }

        public override void Enter()
        {
            if (_targetNode == null)
            {
                Unit.SetState(new IdleState(Unit));
                return;
            }

            Unit.Mover.OnPathComplete += OnPathComplete;
            Unit.Mover.OnDirectionChanged += OnDirectionChanged;
            Unit.Mover.MoveTo(_targetNode, Unit.Speed);
            
            // Animation
            Unit.Animator.PlayMove(Vector2.zero);
        }

        public override void Update() { }

        public override void Exit()
        {
            Unit.Mover.OnPathComplete -= OnPathComplete;
            // Reset the animator state
            Unit.Animator.ResetState();
        }

        private void OnPathComplete()
        {
            if (Unit.CurrentTarget != null)
                Unit.SetState(new AttackState(Unit, Unit.CurrentTarget));
            else
                Unit.SetState(new IdleState(Unit));
        }
        
        private void OnDirectionChanged(Vector2 direction)
        {
            Unit.Animator.ChangeMovingDirection(direction);
        }
        public override string DisplayName => "Move";
    }


}