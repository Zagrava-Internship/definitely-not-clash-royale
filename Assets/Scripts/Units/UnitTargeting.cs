using System;
using Combat.Interfaces;
using Targeting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Units
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class UnitTargeting : MonoBehaviour
    {
        private IAttacker _attacker; // The attacker component that will handle target acquisition 
        [FormerlySerializedAs("_aggroCollider")] [SerializeField]
        public CircleCollider2D aggroCollider;

        public ITargetable CurrentTarget { get; private set; }
        public bool HasTarget => CurrentTarget is { IsTargetDead: false };
        
        public event Action<ITargetable> OnTargetAcquired;
        public event Action<ITargetable> OnTargetLost;

        private void Awake()
        {
            aggroCollider.isTrigger = true; // Ensure the collider is a trigger for detecting targets
        }

        public void InitializeTargeting(IAttacker attacker,float aggressionRange)
        {
            _attacker = attacker;
            aggroCollider.radius = aggressionRange;
        }

        public void SetTarget(ITargetable target)
        {
            CurrentTarget = target;
            OnTargetAcquired?.Invoke(target);
        }
        
        public void ClearTarget() => CurrentTarget = null;


        private void OnTriggerStay2D(Collider2D other)
        {
            if(!other.CompareTag("Stress"))
                return; // Ignore non-stress objects
            var tgt = other.GetComponent<ITargetable>();
            if (tgt == null || tgt.Team == _attacker.Team || tgt.IsTargetDead) return;
            
            if (!HasTarget)
            {
                SetTarget(tgt);
                return;
            }
            var newDistSq = (tgt.ObjectTransform.position - transform.position).sqrMagnitude;
            var curDistSq = (CurrentTarget.ObjectTransform.position - transform.position).sqrMagnitude;

            if (newDistSq + 0.01f < curDistSq)
                SetTarget(tgt);
        }
        
        // Handle when a target exits the aggro range
        private void OnTriggerExit2D(Collider2D other)
        {
            //Debug.Log("OnTriggerExit2D called with: " + other.name);
            if(!other.CompareTag("Stress"))
                return; // Ignore non-stress objects
            var tgt = other.GetComponent<ITargetable>();
            if (tgt == null || tgt.Team == _attacker.Team || tgt.IsTargetDead) return;

            if (CurrentTarget != tgt) return;
            ClearTarget();
            OnTargetLost?.Invoke(tgt);
        }
    }
}