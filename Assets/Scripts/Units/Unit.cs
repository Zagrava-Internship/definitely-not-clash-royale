using Combat;
using Combat.Interfaces;
using Health;
using Maps.MapManagement.Grid;
using Targeting;
using Units.Animation;
using Units.StateMachine;
using Units.Strategies.Attack;
using Units.Strategies.Movement;
using UnityEngine;
using UnityEngine.Serialization;


namespace Units
{
    [RequireComponent(typeof(GridMover), typeof(UnitStats), typeof(UnitTargeting))]
    [RequireComponent(typeof(HealthComponent), typeof(UnitStateMachine))]
    [RequireComponent(typeof(UnitAnimator), typeof(SpriteRenderer), typeof(HealthBarController))]
    public class Unit : TargetableBase,IAttacker
    {
        // Components
        public UnitStats Stats { get; private set; }
        public UnitTargeting Targeting { get; private set; }
        public UnitStateMachine StateMachine { get; private set; }
        private HealthComponent Health { get; set; }
        public GridMover Mover { get; private set; }
        public UnitAnimator Animator { get; private set; }
        
        // Strategies
        public IMovementStrategy MovementStrategy { get; private set; }
        public IAttackStrategy AttackStrategy { get; private set; }
        
        // ITargetable 
        public override Transform Transform => transform;
        public override bool IsDead => Health == null || Health.Current <= 0;
        public ITargetable CurrentTarget => Targeting.CurrentTarget;
        
        // IAttacker properties
        public int Damage => Stats.Damage;

        private SpriteRenderer _spriteRenderer;
        
        public void Initialize(UnitConfig unitConfig, string teamId)
        {
            TeamId = teamId;
            
            // Get components
            Stats = GetComponent<UnitStats>();
            Targeting = GetComponent<UnitTargeting>();
            StateMachine = GetComponent<UnitStateMachine>();
            Health = GetComponent<HealthComponent>();
            Mover = GetComponent<GridMover>();
            Animator = GetComponent<UnitAnimator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            var healthBarController = GetComponent<HealthBarController>();
            
            // Validate components
            Stats.Initialize(unitConfig);
            Targeting.Initialize(Stats.AggressionRange);
            Health.Setup(Stats.MaxHealth);
            healthBarController.Init(Health);
            StateMachine.Initialize(this);
            Animator.Initialize(unitConfig.weaponData.AttackSpeed);
            
            MovementStrategy = GetComponent<IMovementStrategy>();
            AttackStrategy = GetComponent<IAttackStrategy>();
            if (MovementStrategy == null) Debug.LogError($"{name}: missing IMovementStrategy component!");
            if (AttackStrategy == null) Debug.LogError($"{name}: missing IAttackStrategy component!");
            
            Health.OnDied += Die;
            Mover.OnDirectionChanged += RotateFromDirection;
        }
        
        public override void TakeDamage(int damage)
        {
            if (IsDead) return;
            Health?.TakeDamage(damage);
        }
        
        public void OnTargetAcquired(ITargetable target)
        {
            // This method can be used to handle logic when a target is acquired
        }

        private void RotateFromDirection(Vector2 direction)
        {
            _spriteRenderer.flipX = direction.x < 0;
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
