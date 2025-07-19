using Combat.Interfaces;
using Combat.Particles;
using Targeting;
using UnityEngine;

namespace Units.Strategies.Attack
{
    public class RangedAttackStrategy: MonoBehaviour, IAttackStrategy
    {
        [Header("Visuals")]
        [SerializeField] private GameObject projectilePrefab;

        private IAttacker _attacker;

        public void SetProjectilePrefab(GameObject prefab) => projectilePrefab = prefab;

        private void Awake() => _attacker = GetComponent<IAttacker>();

        public float Range => _attacker.AttackRange;
        public float AttackDelay => _attacker.AttackDelay;

        public void Attack(IAttacker attacker,ITargetable target)
        {
            if (target == null || target.IsDead) return;
            ParticleManager.SpawnParticle(projectilePrefab, target.Transform,
                _attacker.Transform,
                () => target.TakeDamage(attacker.Damage));
        }
        
    }
}