using Targeting;
using UnityEngine;

namespace Units.StateMachine
{
    public class AttackState: UnitState
    {
        private readonly ITargetable _target;
        public AttackState(Unit unit, ITargetable target) : base(unit)
        {
            this._target = target;
        }

        public override void Enter()
        {
            Unit.Animator.PlayAttack();
            Unit.Animator.OnAttackAnimationEnd+= HandleAttackAnimationEnd;
        }

        private void HandleAttackAnimationEnd()
        {
            Unit.Animator.OnAttackAnimationEnd -= HandleAttackAnimationEnd;
            
            if (_target == null || _target.IsDead || _target != Unit.CurrentTarget)
            {
                Unit.StateMachine.SetState(new IdleState(Unit));
                return;
            }
            if (Vector3.Distance(Unit.Transform.position,
                    _target.Transform.position) > Unit.AttackStrategy.Range)
            {
                Unit.StateMachine.SetState(new MoveState(Unit));
                return;
            }
            
            Unit.AttackStrategy.Attack(Unit, _target);
            
            //Unit.Animator.PlayAttack();
            Unit.Animator.OnAttackAnimationEnd += HandleAttackAnimationEnd;
        }

        public override void Update()
        {
            if(_target== null || _target.IsTargetDead)
                Unit.StateMachine.SetState(new IdleState(Unit));
        }

        public override void Exit()
        {
            Unit.Animator.OnAttackAnimationEnd -= HandleAttackAnimationEnd;
            Unit.Animator.ResetState();
        }
        
        public override string DisplayName => "Attack";

    }
}