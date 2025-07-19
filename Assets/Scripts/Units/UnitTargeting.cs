using System;
using Combat.Interfaces;
using Targeting;
using UnityEngine;

namespace Units
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class UnitTargeting : MonoBehaviour
    {
        private IAttacker _attacker; // The attacker component that will handle target acquisition 
        private CircleCollider2D _aggroCollider;

        public ITargetable CurrentTarget { get; private set; }
        public bool HasTarget => CurrentTarget is { IsDead: false };
        
        public event Action<ITargetable> OnTargetAcquired;
        public event Action<ITargetable> OnTargetLost;

        private void Awake()
        {
            _aggroCollider = GetComponent<CircleCollider2D>();
            _aggroCollider.isTrigger = true; // Ensure the collider is a trigger for detecting targets
        }

        public void Initialize(IAttacker attacker,float aggressionRange)
        {
            _attacker = attacker;
            _aggroCollider.radius = aggressionRange;
        }

        public void SetTarget(ITargetable target)
        {
            CurrentTarget = target;
            OnTargetAcquired?.Invoke(target);
        }
        
        public void ClearTarget() => CurrentTarget = null;


        private void OnTriggerStay2D(Collider2D other)
        {
            //Debug.Log("OnTriggerStay2D called with: " + other.name);
            var tgt = other.GetComponent<ITargetable>();
            if (tgt == null || tgt.Team == _attacker.Team || tgt.IsDead) return;
            
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
        
        // Handle when a target exits the aggro range
        private void OnTriggerExit2D(Collider2D other)
        {
            //Debug.Log("OnTriggerExit2D called with: " + other.name);
            var tgt = other.GetComponent<ITargetable>();
            if (tgt == null || tgt.Team == _attacker.Team || tgt.IsDead) return;

            if (CurrentTarget != tgt) return;
            ClearTarget();
            OnTargetLost?.Invoke(tgt);
        }
    }
}