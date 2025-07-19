using Targeting;
using UnityEngine;

namespace Units
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class UnitTargeting : MonoBehaviour
    {
        [SerializeField] private Unit unit; // The unit that this targeting component belongs to
        private CircleCollider2D _aggroCollider;

        public ITargetable CurrentTarget { get; private set; }
        public bool HasTarget => CurrentTarget is { IsDead: false };

        private void Awake()
        {
            _aggroCollider = GetComponent<CircleCollider2D>();
            if (unit == null) unit = GetComponentInParent<Unit>();
        }

        public void Initialize(float aggressionRange)
        {
            _aggroCollider.radius = aggressionRange;
        }

        public void SetTarget(ITargetable target)
        {
            CurrentTarget = target;
            unit.OnTargetAcquired(target);
        }
        
        public void ClearTarget() => CurrentTarget = null;


        private void OnTriggerStay2D(Collider2D other)
        {
            var tgt = other.GetComponent<ITargetable>();
            if (tgt == null || tgt.TeamId == unit.TeamId || tgt.IsDead) return;
            
            if (!HasTarget)
            {
                SetTarget(tgt);
                return;
            }
            var newDistSq = (tgt.Transform.position - transform.position).sqrMagnitude;
            var curDistSq = (CurrentTarget.Transform.position - transform.position).sqrMagnitude;

            if (newDistSq + 0.01f < curDistSq)
                SetTarget(tgt);
        }
    }
}