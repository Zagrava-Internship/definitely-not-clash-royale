using Maps.MapManagement.Grid;
using Targeting;
using UnityEngine;

namespace Units.UnitStates
{
    public class IdleState : UnitState
    {
        public IdleState(Unit unit) : base(unit) { }

        public override void Enter()
        {
            Unit.Animator.PlayIdle();
        }

        public override void Update()
        {
            if (Unit.CurrentTarget == null || Unit.CurrentTarget.IsDead)
            {
                var enemy = TargetFinder.FindClosestEnemy(
                    Unit.transform.position,
                    Unit.TeamId
                );

                if (enemy != null) Unit.SetTarget(enemy);
                else return;
            }
            
            if (Unit.CurrentTarget == null)
            {
                Debug.Log("CurrentTarget is null in IdleState but should not be.");
                return;
            }
            
            var dist = Vector3.Distance(
                Unit.transform.position,
                Unit.CurrentTarget.Transform.position
            );

            if (dist <= Unit.AttackStrategy.Range)
            {
                Unit.SetState(new AttackState(Unit, Unit.CurrentTarget));
            } else {
                var node = GridManager.Instance
                    .GetNodeFromWorldPoint(Unit.CurrentTarget.Transform.position);
                if (node != null) Unit.SetState(new MoveState(Unit, node));
            }
        }

        public override void Exit()
        {
            Unit.Animator.ResetState();
        }


        public override string DisplayName => "Idle";
    }

}