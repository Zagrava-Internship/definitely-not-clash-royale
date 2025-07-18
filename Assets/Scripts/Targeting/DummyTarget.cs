using Health;
using UnityEngine;

namespace Targeting
{
    public class DummyTarget : TargetableBase
    {
        [SerializeField] private int hp = 100;
        private HealthComponent Health { get; set; }
        private HealthBarController HealthBarController { get; set; }
        public override Transform Transform => transform;
        public override bool IsDead => Health.Current<=0;

        protected override void OnEnable()
        {
            TargetRegistry.AllTargets.Add(this);
            HealthBarController= GetComponent<HealthBarController>();
            
            Health = GetComponent<HealthComponent>();
            Health.OnDied+= Die;
            Health.Setup(hp);
            
            HealthBarController.Init(Health);
        }

        public override void TakeDamage(int damage)
        {
            if (IsDead) return;
            Health.TakeDamage(damage);
        }
        private void Die()
        {
            Destroy(gameObject);
        }
    }

}