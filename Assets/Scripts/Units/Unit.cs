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
        public override Transform ObjectTransform => transform;
        public override bool IsTargetDead => Health == null || Health.Current <= 0;
        public ITargetable AttackerCurrentTarget => Targeting.CurrentTarget;
        
        // IAttacker properties
        public int AttackerDamage => Stats.Damage;
        public float AttackerRange=> Stats.AttackRange;
        public float AttackerDelay=> Stats.AttackDelay;

        private SpriteRenderer _spriteRenderer;
        
        public void InitializeUnit(UnitConfig unitConfig, Team team)
        {
            Team = team;
            
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
            Stats.InitializeStats(unitConfig);
            Targeting.InitializeTargeting(this,Stats.AggressionRange);
            Health.Setup(Stats.MaxHealth);
            healthBarController.Init(Health,team);
            StateMachine.InitializeStateMachine(this);
            Animator.Initialize(unitConfig.weaponData.AttackSpeed);
            
            MovementStrategy = GetComponent<IMovementStrategy>();
            AttackStrategy = GetComponent<IAttackStrategy>();
            if (MovementStrategy == null) Debug.LogError($"{name}: missing IMovementStrategy component!");
            if (AttackStrategy == null) Debug.LogError($"{name}: missing IAttackStrategy component!");
            
            Health.OnDied += Die;
            Mover.OnDirectionChanged += RotateFromDirection;
        }
        
        public override void ApplyDamage(int damage)
        {
            if (IsTargetDead) return;
            Health?.TakeDamage(damage);
        }

        public void OnTargetAcquired(ITargetable target)
        {
            // This method can be used to handle logic when a target is acquired
        }

        public void OnTargetLost(ITargetable target)
        {
            // This method can be used to handle logic when a target is lost
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
