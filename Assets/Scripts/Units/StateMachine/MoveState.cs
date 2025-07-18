using Maps.MapManagement.Grid;
using UnityEngine;

namespace Units.StateMachine
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

            Unit.MovementStrategy.Move(Unit, _targetNode.WorldPosition, Unit.Speed);
            
            // Animation
            Unit.Animator.PlayMove(Vector2.zero);
        }

        public override void Update() { }

        public override void Exit()
        {
            Unit.MovementStrategy.Stop(Unit);

            // Reset the animator state
            Unit.Animator.ResetState();
        }
        
        public override string DisplayName => "Move";
    }


}