using System;
using Health;
using Targeting;
using UnityEngine;

namespace Units
{
    public class DummyTarget : TargetableBase
    {
        [SerializeField] private int hp = 100;
        public HealthBarController HealthBarController { get; private set; }
        public override Vector3 Position => transform.position;
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

        private void Die()
        {
            Destroy(gameObject);
        }
    }

}