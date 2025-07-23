using Health;
using UnityEngine;

namespace Targeting
{
    public class DummyTarget : TargetableBase
    {
        [SerializeField] private int hp = 100;
        private HealthComponent Health { get; set; }
        private HealthBarController HealthBarController { get; set; }
        public override Transform ObjectTransform => transform;
        public override bool IsTargetDead => Health.Current<=0;

        protected override void OnEnable()
        {
            TargetRegistry.AllTargets.Add(this);
            HealthBarController= GetComponent<HealthBarController>();
            
            Health = GetComponent<HealthComponent>();
            Health.OnDied+= Die;
            Health.Setup(hp);
            
            // Since this is a dummy target, we can set the team directly
            HealthBarController.Init(Health,Team);
        }

        public override void ApplyDamage(int damage)
        {
            if (IsTargetDead) return;
            Health.TakeDamage(damage);
        }
        private void Die()
        {
            Destroy(gameObject);
        }
    }

}