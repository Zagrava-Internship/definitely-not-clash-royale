using Maps.MapManagement.Grid;
using UnityEngine;

namespace Units.StateMachine
{
    public class MoveState : UnitState
    {

        private const float RepathDistanceSq = 0.5f;
        private Vector3 _lastTargetPos;
        public MoveState(Unit unit) : base(unit)
        { }

        public override void Enter()
        {
            _lastTargetPos = Unit.CurrentTarget.Transform.position;
            Repath();
            Unit.Animator.PlayMove(Vector2.zero);
        }

        public override void Update()
        {
            if (Unit.CurrentTarget == null || Unit.CurrentTarget.IsDead)
            {
                Unit.StateMachine.SetState(new IdleState(Unit));
                return;
            }

            var delta = Unit.CurrentTarget.Transform.position - _lastTargetPos;
            if (delta.sqrMagnitude >= RepathDistanceSq)
            {
                _lastTargetPos = Unit.CurrentTarget.Transform.position;
                Repath();
            }

            if (Vector3.Distance(Unit.transform.position, _lastTargetPos) <= Unit.AttackStrategy.Range)
                Unit.StateMachine.SetState(new AttackState(Unit, Unit.CurrentTarget));
        }

        public override void Exit()
        {
            Unit.MovementStrategy.Stop(Unit);

            // Reset the animator state
            Unit.Animator.ResetState();
        }
        
        private void Repath()
        {
            var node = GridManager.Instance.GetNodeFromWorldPoint(_lastTargetPos);
            if (node != null)
                Unit.MovementStrategy.Move(Unit, node.WorldPosition, Unit.Stats.Speed);
        }
        
        public override string DisplayName => "Move";
    }


}