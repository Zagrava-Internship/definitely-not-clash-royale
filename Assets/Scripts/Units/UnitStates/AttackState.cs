using Targeting;
using UnityEngine;

namespace Units.UnitStates
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
            Unit.AttackStrategy.Attack(Unit, _target);
            if (_target is { IsDead: false })
            {
                Unit.Animator.PlayAttack();
                Unit.Animator.OnAttackAnimationEnd += HandleAttackAnimationEnd;
            } else
            {
                Unit.SetState(new IdleState(Unit));
            }
            
            Unit.Animator.PlayAttack();
            Unit.Animator.OnAttackAnimationEnd += () =>
                Unit.AttackStrategy.Attack(Unit, _target);
        }

        public override void Update()
        {
            if(_target== null || _target.IsDead)
                Unit.SetState(new IdleState(Unit));
        }

        public override void Exit()
        {
            Unit.Animator.OnAttackAnimationEnd -= HandleAttackAnimationEnd;
            Unit.Animator.ResetState();
        }
        
        public override string DisplayName => "Attack";

    }
}