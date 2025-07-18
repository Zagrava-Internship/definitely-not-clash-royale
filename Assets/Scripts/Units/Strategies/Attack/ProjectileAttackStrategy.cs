using Combat.Particles;
using Targeting;
using UnityEngine;

namespace Units.Strategies.Attack
{
    public class ProjectileAttackStrategy: MonoBehaviour, IAttackStrategy
    {
        [Header("Visuals")]
        [SerializeField] private GameObject projectilePrefab;

        private Unit _unit;

        private void Awake() => _unit = GetComponent<Unit>();

        public float Range => _unit.AttackRange;
        public float AttackDelay => _unit.AttackDelay;

        public void Attack(Unit unit, ITargetable target)
        {
            if (target == null || target.IsDead) return;
            ParticleManager.SpawnParticle(projectilePrefab, target.Transform,
                () => target.TakeDamage(unit.Damage));
        }

    }
}