using Units.Enums;
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
        
        public MovementType MovementType { get; protected set; }

        protected virtual void OnEnable()  => TargetRegistry.AllTargets.Add(this);
        protected virtual void OnDisable() => TargetRegistry.AllTargets.Remove(this);
        
        public abstract Transform ObjectTransform { get; }
        public abstract bool IsTargetDead { get; }
        public abstract void ApplyDamage(int damage);
        public bool CanBeTargeted(Team otherTeam, MovementType otherMovementType)
        {
            return otherTeam != Team &&
                   (MovementType == MovementType.Ground ||
                    MovementType == MovementType.Static ||
                    !(otherMovementType == MovementType.Ground && MovementType == MovementType.Flying));
        }
    }

}