using UnityEngine;
using UnityEngine.Serialization;

namespace Targeting
{
    public abstract class TargetableBase : MonoBehaviour, ITargetable
    {
        [FormerlySerializedAs("teamId")]
        [Header("Allegiance")]
        [SerializeField]
        private Team team; 
        public Team Team
        {
            get => team;
            protected set => team = value;
        }
        protected virtual void OnEnable()  => TargetRegistry.AllTargets.Add(this);
        protected virtual void OnDisable() => TargetRegistry.AllTargets.Remove(this);
        
        public abstract Transform Transform { get; }
        public abstract bool IsDead { get; }
        public abstract void TakeDamage(int damage);
    }

}