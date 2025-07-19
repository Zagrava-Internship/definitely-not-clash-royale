using Combat;
using Combat.Interfaces;
using Health;
using Targeting;
using Towers.Animation;
using Units;
using Units.Strategies.Attack;
using UnityEngine;

namespace Towers
{
    [RequireComponent(typeof(UnitStats), typeof(UnitTargeting))]
    [RequireComponent(typeof(HealthComponent),typeof(HealthBarController))]
    [RequireComponent(typeof(IAttackStrategy))]
    public class Tower : TargetableBase,IAttacker
    {
       public UnitStats Stats { get; private set; }
        public UnitTargeting Targeting { get; private set; }
        public HealthComponent Health { get; private set; }
        public IAttackStrategy AttackStrategy { get; private set; }
        private TowerAnimator _animator;

        // Prevents starting a new attack while one is already in progress.
        private bool _isAttacking;

        // IAttacker properties
        public int Damage => Stats.Damage;
        public float AttackRange => Stats.AttackRange;
        public float AttackDelay => Stats.AttackDelay; // Meant to be used by an external cooldown system.

        // ITargetable
        public override Transform Transform => transform;
        public override bool IsDead => Health == null || Health.Current <= 0;
        public ITargetable CurrentTarget => Targeting.CurrentTarget;

        public void Initialize(UnitConfig towerConfig, Team team)
        {
            Team = team;
            
            Stats = GetComponent<UnitStats>();
            Targeting = GetComponent<UnitTargeting>();
            Health = GetComponent<HealthComponent>();
            AttackStrategy = GetComponent<IAttackStrategy>();
            _animator = GetComponent<TowerAnimator>();

            Stats.Initialize(towerConfig);
            Targeting.Initialize(this, Stats.AggressionRange);
            Health.Setup(Stats.MaxHealth);
            _animator.Initialize(Stats.AttackDelay);

            GetComponent<HealthBarController>()?.Init(Health);

            Health.OnDied += Die;
            _animator.OnAttackAnimationEnd += HandleAttackAnimationEnd;
            Targeting.OnTargetAcquired += OnTargetAcquired;
            Targeting.OnTargetLost += OnTargetLost;
        }
        
        private void OnDestroy()
        {
            // Unsubscribe from events to prevent memory leaks.
            if (Health != null) Health.OnDied -= Die;
            if (_animator != null) _animator.OnAttackAnimationEnd -= HandleAttackAnimationEnd;
        }

        private void Update()
        {
            // If the target dies mid-attack, reset the attack state.
            if (_isAttacking && (CurrentTarget == null || CurrentTarget.IsDead))
            {
                ResetAttackState();
            }
        }

        public override void TakeDamage(int damage)
        {
            if (IsDead) return;
            Health.TakeDamage(damage);
        }

        public void OnTargetAcquired(ITargetable target)
        {
            _isAttacking = true;
            _animator.PlayAttack();
        }

        public void OnTargetLost(ITargetable target)
        {
            ResetAttackState();
        }

        /// <summary>
        /// Called by the animator event when the attack animation completes.
        /// </summary>
        private void HandleAttackAnimationEnd()
        {
            if (CurrentTarget != null && !CurrentTarget.IsDead)
            {
                AttackStrategy.Attack(this, CurrentTarget);
            }
        }

        private void ResetAttackState()
        {
            _isAttacking = false;
            _animator.ResetState();
        }

        private void Die()
        {
            Debug.Log($"{name} destroyed!");
            Destroy(gameObject);
        }
    }
}