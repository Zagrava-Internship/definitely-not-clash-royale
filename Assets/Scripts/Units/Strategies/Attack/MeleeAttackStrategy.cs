using Combat.Interfaces;
using Targeting;
using UnityEngine;

namespace Units.Strategies.Attack
{
    [RequireComponent(typeof(Unit))]
    public class MeleeAttackStrategy: MonoBehaviour, IAttackStrategy
    {
        public float Range => _unit.Stats.AttackRange;
        public float AttackDelay => _unit.Stats.AttackDelay;

        private Unit _unit;

        private void Awake() => _unit = GetComponent<Unit>();

        public void Attack(IAttacker attacker,ITargetable target)
        {
            if (target == null || target.IsTargetDead) return;
            if (Vector3.Distance(attacker.ObjectTransform.position, target.ObjectTransform.position) > Range) return;

            target.ApplyDamage(attacker.AttackerDamage);
        }
    }
}