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
        public float Damage => data.damage;
        public UnitType Type => data.type;

        public GridMover Mover { get; private set; } 
        public UnitAnimator Animator { get; private set; }
        public ITargetable CurrentTarget { get; private set; }
        public void SetTarget(ITargetable target) => CurrentTarget = target;
        
        private float _currentHealth;
        private UnitState _state;          
        private SpriteRenderer _spriteRenderer;

        public void Initialize(UnitData unitData)
        {
            if (!unitData)
            {
                throw new System.ArgumentNullException(nameof(unitData), "Unit.Initialize: UnitData is null");
            }
            data = unitData;
            _currentHealth = data.health;
            Mover = GetComponent<GridMover>();
            Animator = GetComponent<UnitAnimator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            
            Mover.OnDirectionChanged += RotateFromDirection;
            SetState(new IdleState(this));
        }
        
        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
            if (_currentHealth <= 0)
            {
                Die();
            }
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
