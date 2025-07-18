using Targeting;
using UnityEngine;

namespace Units.Strategies.Attack
{
    [RequireComponent(typeof(Unit))]
    public class MeleeAttackStrategy: MonoBehaviour, IAttackStrategy
    {
        public float Range => _unit.AttackRange;
        public float AttackDelay => _unit.AttackDelay;

        private Unit _unit;

        private void Awake() => _unit = GetComponent<Unit>();

        public void Attack(Unit unit, ITargetable target)
        {
            if (target == null || target.IsDead) return;
            if (Vector3.Distance(unit.transform.position, target.Transform.position) > Range) return;

            target.TakeDamage(unit.Damage);
        }
    }
}