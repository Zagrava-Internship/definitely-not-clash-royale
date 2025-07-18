using Combat;
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
    [RequireComponent(typeof(GridMover))]
    public class Unit : TargetableBase
    {
        [FormerlySerializedAs("data")]
        [Header("Base data")]
        [SerializeField] private UnitConfig config;
        
        public IMovementStrategy MovementStrategy { get; private set; }
        public IAttackStrategy   AttackStrategy   { get; private set; }

        public float MaxHealth => config.health;
        public float Speed => config.speed;
        public int Damage => config.weaponData.Damage;
        public float AttackRange => config.weaponData.AttackRange;
        public float AttackSpeed => config.weaponData.AttackSpeed;
        public float AttackDelay => config.weaponData.AttackDelay;
        public float AggressionRange => config.aggressionRange;
        //public UnitType Type => data.type;

        public GridMover Mover { get; private set; } 
        public UnitAnimator Animator { get; private set; }
        public ITargetable CurrentTarget { get; private set; }
        private HealthComponent Health { get; set; }
        public WeaponComponent Weapon { get; private set; }
        private HealthBarController HealthBarController { get; set; }
     
        public override Transform Transform => transform;
        public override bool IsDead => Health == null || Health.Current <= 0;

        public override void TakeDamage(int damage)
        {
            if (IsDead) return;
            Health?.TakeDamage(damage);
        }
        public void SetTarget(ITargetable target) => CurrentTarget = target;
        
        private UnitState _state;          
        private SpriteRenderer _spriteRenderer;

        
        
        public void Initialize(UnitConfig unitConfig, string teamId)
        {
            if (!unitConfig)
            {
                throw new System.ArgumentNullException(nameof(unitConfig), "Unit.Initialize: UnitData is null");
            }
            config = unitConfig;
            
            TeamId = teamId;

            MovementStrategy = GetComponent<IMovementStrategy>();
            AttackStrategy = GetComponent<IAttackStrategy>();
            
            if (MovementStrategy == null)
                Debug.LogError($"{name}: missing IMovementStrategy component!");
            if (AttackStrategy == null)
                Debug.LogError($"{name}: missing IAttackStrategy component!");
            
            // Set up the unit's properties based on the provided UnitData
            var agreeCollider = GetComponent<CircleCollider2D>();
            agreeCollider.radius = config.aggressionRange;
            
            Mover = GetComponent<GridMover>();
            Animator = GetComponent<UnitAnimator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            HealthBarController= GetComponent<HealthBarController>();
            
            Health = GetComponent<HealthComponent>();
            Health.OnDied+= Die;
            Health.Setup(unitConfig.health);
            
            HealthBarController.Init(Health);
            
            Weapon = GetComponent<WeaponComponent>();
            if (Weapon is null)
                Debug.LogError("Unit: WeaponComponent is not assigned. Please assign a weapon component to the unit.");
            
            Mover.OnDirectionChanged += RotateFromDirection;
            SetState(new IdleState(this));
        }
        
        private void RotateFromDirection(Vector2 direction)
        {
            _spriteRenderer.flipX = direction.x < 0;
        }

        private void Update() =>  _state?.Update();

        public UnitState GetState()
        {
           return _state;
        }
        public void SetState(UnitState next)
        {
            _state?.Exit();
            _state = next;
            _state.Enter();
        }
        private void Die()
        {
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (CurrentTarget is null)
                return;
            if(!other.CompareTag("Enemy"))
                return;
            if (other.transform.position == CurrentTarget.Transform.position)
            {
                // Log the distance between the unit and the target
                Debug.Log($"Unit {name} collided with target {CurrentTarget.Transform.name} at position {other.transform.position}. " +
                          $"Distance to target: {Vector2.Distance(transform.position, CurrentTarget.Transform.position)}");
                // TODO : // Add to target registry or handle the target
                // Placeholder implementation
                if (other.TryGetComponent(out ITargetable target))
                {
                    SetTarget(target);
                }
            }
            else
            {
                Debug.LogWarning($"Unexpected collision with {other.name} at position {other.transform.position}. " +
                                 $"Expected target position: {CurrentTarget.Transform.position}");
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Enemy")) return;
            if(_state is AttackState)return;
            var distance = Vector2.Distance(transform.position, other.transform.position);
            if (!(distance <= AttackRange)) return;
            //Debug.Log("Unit is within attack range of the target.");
            Mover.ForceToStop();
        }
    }
}
