using Combat.Interfaces;
using Combat.Particles;
using Targeting;
using UnityEngine;

namespace Units.Strategies.Attack
{
    public class ParticleAttackStrategy: MonoBehaviour, IAttackStrategy
    {
        [Header("Visuals")]
        [SerializeField] private GameObject projectilePrefab;

        private Unit _unit;

        public void SetProjectilePrefab(GameObject prefab) => projectilePrefab = prefab;

        private void Awake() => _unit = GetComponent<Unit>();

        public float Range => _unit.Stats.AttackRange;
        public float AttackDelay => _unit.Stats.AttackDelay;

        public void Attack(IAttacker attacker,ITargetable target)
        {
            if (target == null || target.IsDead) return;
            ParticleManager.SpawnParticle(projectilePrefab, target.Transform,
                () => target.TakeDamage(attacker.Damage));
        }
        
    }
}