using Targeting;
using UnityEngine;

namespace Units
{
    public abstract class TargetableBase : MonoBehaviour, ITargetable
    {
        protected virtual void OnEnable()  => TargetRegistry.AllTargets.Add(this);
        protected virtual void OnDisable() => TargetRegistry.AllTargets.Remove(this);
        
        public abstract Vector3 Position { get; }
        public abstract void TakeDamage(float amount);
        public abstract bool IsDead { get; }
    }

}