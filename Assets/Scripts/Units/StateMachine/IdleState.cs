using Targeting;
using UnityEngine;

namespace Units.StateMachine
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
            if (Unit.AttackerCurrentTarget == null || Unit.AttackerCurrentTarget.IsTargetDead)
            {
                var enemy = TargetFinder.FindClosestEnemy(
                    Unit.transform.position,
                    Unit.Team
                );

                if (enemy != null) Unit.Targeting.SetTarget(enemy);
                else return;
            }
            
            if (Unit.AttackerCurrentTarget == null)
            {
                Debug.Log("AttackerCurrentTarget is null in IdleState but should not be.");
                return;
            }
            
            var dist = Vector3.Distance(
                Unit.transform.position,
                Unit.AttackerCurrentTarget.ObjectTransform.position
            );

            if (dist <= Unit.AttackStrategy.Range)
            {
                Unit.StateMachine.SetState(new AttackState(Unit, Unit.AttackerCurrentTarget));
            } else {
                Unit.StateMachine.SetState(new MoveState(Unit));
            }
        }

        public override void Exit()
        {
            Unit.Animator.ResetState();
        }


        public override string DisplayName => "Idle";
    }

}