using Units.Enums;
using UnityEngine;

namespace Targeting
{
    /// <summary>
    /// Contract for all entities that can be targeted and damaged by units (e.g., units, buildings, etc.).
    /// </summary>
    public interface ITargetable: ITeamEntity
    {
        // ObjectTransform of the target
        Transform ObjectTransform { get; }
        MovementType MovementType { get; }
        
        bool IsTargetDead { get; }
        
        // Method to apply damage to the target
        void ApplyDamage(int damage);
        bool CanBeTargeted(Team otherTeam, MovementType otherMovementType);
    }
}