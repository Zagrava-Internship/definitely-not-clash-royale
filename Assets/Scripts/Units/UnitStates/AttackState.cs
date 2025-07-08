using UnityEngine;

namespace Units.UnitStates
{
    public class AttackState: UnitState
    {
        private readonly ITargetable _target;
        private const float AttackCooldown = 1f;
        private float _timer = 0f;

        public AttackState(Unit unit, ITargetable target) : base(unit)
        {
            this._target = target;
        }

        public override void Enter()
        {
            _timer = 0f;
        }

        public override void Update()
        {
            if (_target == null || _target.IsDead)
            {
                Unit.SetState(new IdleState(Unit));
                return;
            }
            _timer += Time.deltaTime;
            if (_timer >= AttackCooldown)
            {
                _timer = 0f;
                _target.TakeDamage(Unit.Damage);
            }
        }

        public override void Exit() { }

    }
}