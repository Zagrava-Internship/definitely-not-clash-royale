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

        public float Range => _attacker.AttackerRange;
        public float AttackDelay => _attacker.AttackerDelay;

        public void Attack(IAttacker attacker,ITargetable target)
        {
            if (target == null || target.IsTargetDead) return;
            ParticleManager.SpawnParticle(projectilePrefab, target.ObjectTransform,
                _attacker.ObjectTransform,
                () => target.ApplyDamage(attacker.AttackerDamage));
        }
        
    }
}