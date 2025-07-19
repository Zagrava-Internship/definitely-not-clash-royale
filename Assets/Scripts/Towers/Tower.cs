using Combat;
using Combat.Interfaces;
using Health;
using Targeting;
using Units;
using Units.Strategies.Attack;
using UnityEngine;

namespace Towers
{
    [RequireComponent(typeof(UnitStats), typeof(UnitTargeting))]
    [RequireComponent(typeof(HealthComponent), typeof(WeaponComponent))]
    public class Tower : TargetableBase,IAttacker
    {
        public UnitStats Stats { get; private set; }
        public UnitTargeting Targeting { get; private set; }
        public HealthComponent Health { get; private set; }
        public WeaponComponent Weapon { get; private set; }
        public IAttackStrategy AttackStrategy { get; private set; }
        
        // IAttacker properties
        public int Damage=> Stats.Damage;
        
        // ITargetable
        public override Transform Transform => transform;
        public override bool IsDead => Health == null || Health.Current <= 0;
        public ITargetable CurrentTarget => Targeting.CurrentTarget;

        public void Initialize(UnitConfig towerConfig, string teamId)
        {
            TeamId = teamId;
            
            Stats = GetComponent<UnitStats>();
            Targeting = GetComponent<UnitTargeting>();
            Health = GetComponent<HealthComponent>();
            Weapon = GetComponent<WeaponComponent>();
            AttackStrategy = GetComponent<IAttackStrategy>();
            var healthBarController = GetComponent<HealthBarController>(); 
            
            Stats.Initialize(towerConfig);
            Targeting.Initialize(Stats.AggressionRange);
            Health.Setup(Stats.MaxHealth);
            if (healthBarController != null) healthBarController.Init(Health);
            
            Health.OnDied += Die;
        }

        public override void TakeDamage(int damage)
        {
            if (IsDead) return;
            Health?.TakeDamage(damage);
        }

        private void Update()
        {
            if (!IsDead && Targeting.HasTarget)
            {
                AttackStrategy.Attack(this, Targeting.CurrentTarget); 
            }
        }
        
        private void Die()
        {
            Debug.Log($"{name} destroyed!");
            Destroy(gameObject);
        }

    }
}