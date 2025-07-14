using Health;
using Maps.MapManagement.Grid;
using Units.Animation;
using Units.UnitStates;
using UnityEngine;
using UnityEngine.Serialization;

namespace Units
{
    [RequireComponent(typeof(GridMover))]
    public class Unit : MonoBehaviour
    {
        [Header("Base data")]
        [SerializeField] private UnitData data;
        
        public float MaxHealth => data.health;
        public float Speed => data.speed;
        public int Damage => data.damage;
        public UnitType Type => data.type;

        public GridMover Mover { get; private set; } 
        public UnitAnimator Animator { get; private set; }
        public ITargetable CurrentTarget { get; private set; }
        public HealthComponent Health { get; private set; }
        public HealthBarController HealthBarController { get; private set; }
        
        public void SetTarget(ITargetable target) => CurrentTarget = target;
        
        private UnitState _state;          
        private SpriteRenderer _spriteRenderer;

        public void Initialize(UnitData unitData)
        {
            if (!unitData)
            {
                throw new System.ArgumentNullException(nameof(unitData), "Unit.Initialize: UnitData is null");
            }
            data = unitData;
            Mover = GetComponent<GridMover>();
            Animator = GetComponent<UnitAnimator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            HealthBarController= GetComponent<HealthBarController>();
            
            Health = GetComponent<HealthComponent>();
            Health.OnDied+= Die;
            Health.Setup(unitData.health);
            
            HealthBarController.Init(Health);
            
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
            if (other.transform.position == CurrentTarget.Position)
            {
                Debug.Log($"Reached target position: {CurrentTarget.Position}. Stopping movement.");
                Mover.ForceToStop();// Stop the movement coroutine
            }
            else
            {
                Debug.LogWarning($"Unexpected collision with {other.name} at position {other.transform.position}. " +
                                 $"Expected target position: {CurrentTarget.Position}");
            }
        }
    }
}
