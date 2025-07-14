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
            Unit.Animator.OnAttackAnimationEnd+= OnAttackAnimationEnd;
        }

        private void OnAttackAnimationEnd()
        {
            _target.Health.TakeDamage(Unit.Damage);
        }

        public override void Update()
        {
            if(_target== null || _target.IsDead)
                Unit.SetState(new IdleState(Unit));
        }

        public override void Exit()
        {
            Unit.Animator.OnAttackAnimationEnd -= OnAttackAnimationEnd;
            Unit.Animator.ResetState();
        }
        
        public override string DisplayName => "Attack";

    }
}