using UnityEngine;

namespace Targeting
{
    public abstract class TargetableBase : MonoBehaviour, ITargetable
    {
        protected virtual void OnEnable()  => TargetRegistry.AllTargets.Add(this);
        protected virtual void OnDisable() => TargetRegistry.AllTargets.Remove(this);
        
        public abstract Transform Transform { get; }
        public abstract bool IsDead { get; }
        public abstract void TakeDamage(int damage);
    }

}